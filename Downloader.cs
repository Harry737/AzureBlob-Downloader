using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace AzureBlob_Download
{
    internal class Downloader
    {
        public class Details
        {
            public string ContainerName { get; set; }
            public long FolderCount { get; set; }
            public long FileCount { get; set; }
        }

        public static int totalfile = 0;
        public static int totalfolder = 0;
        public static List<Details> det = new List<Details>();
        public static BackupInProgress _form;
        public static Form3 _form3;

        public Downloader(BackupInProgress form)
        {
            _form = form;
            
        }

        public Downloader(Form3 form3)
        {
            _form3 = form3;

        }

        public static async Task<bool> Download(string connectionString,string downloadPath)
        {
            det.Clear();
            string workingDirectory = System.IO.Path.Combine(downloadPath , "AzureBlob-Details.xlsx");
            var Blob_Conn = connectionString;
            var Path = downloadPath;

            List<Task> tasks = new List<Task>();
            try
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                BlobServiceClient _blobServiceClient = new BlobServiceClient(Blob_Conn);
                var containerList = _blobServiceClient.GetBlobContainers();
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Blob_Conn);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                var progress = 0;
                var percentage = 100/containerList.Count();
                foreach (var container in containerList)
                {
                    string absolutePath = System.IO.Path.Combine(Path, container.Name);
                    await BlobToLocal(blobClient, container.Name, absolutePath);
                    _form.IncrementMyProgressBarValue(percentage);
                    progress += percentage;
                }
                _form.IncrementMyProgressBarValue(100-progress);
                _form3.SetMyTextBoxText(totalfile.ToString());
                _form3.SetMyTextBoxText2(totalfolder.ToString());
                _form3.SetMyTextBoxText3("Blob Details available in the following excel : " + workingDirectory);
                //ExportToExcel(det, workingDirectory);
                //_form.SetMyTextBoxText("Blob Details available in the following excel : " + workingDirectory);
                //_form.ChangeColor(System.Drawing.Color.Green);
                //_form.SetMyTextBoxText1("Download Completed");
                //_form.ChangeColor1(System.Drawing.Color.Green);
                //await Task.WhenAll(tasks);
                return true;
            }
            catch (Exception ex)
            {
                _form.SetMyTextBoxText("Exception occured : " + ex.Message);
                _form.ChangeColor(System.Drawing.Color.Red);
                return false;
            }

        }
        public static async Task BlobToLocal(CloudBlobClient blobClient, string containerName, string localPath)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            BlobContinuationToken continuationToken = null;
            BlobResultSegment resultSegment = null;

            int folderCount = 0;
            int fileCount = 0;

            HashSet<string> uniqueFolders = new HashSet<string>();

            do
            {
                resultSegment = await container.ListBlobsSegmentedAsync(null, true, BlobListingDetails.Metadata, null, continuationToken, null, null);
                continuationToken = resultSegment.ContinuationToken;

                foreach (var blobItem in resultSegment.Results)
                {
                    
                    if (blobItem is CloudBlockBlob blockBlob)
                    {
                        fileCount++;
                        totalfile++;
                        string[] pathSegments = blockBlob.Name.Split('/');
                        if (pathSegments.Length > 1)
                        {
                            totalfolder+= pathSegments.Length - 1;
                            folderCount += pathSegments.Length - 1;
                        }

                        string localFilePath = Path.Combine(localPath, blockBlob.Name);
                        Directory.CreateDirectory(Path.GetDirectoryName(localFilePath));

                        _form.SetMyTextBoxText($"Downloading {blockBlob.Name}");
                        //Console.WriteLine($"Downloading {blockBlob.Name} to {localFilePath}");
                        await blockBlob.DownloadToFileAsync(localFilePath, FileMode.Create);
                        //Console.WriteLine($"Downloaded {blockBlob.Name} to {localFilePath}");
                    }
                }
            }
            while (continuationToken != null);
            det.Add(new Details { ContainerName = containerName, FileCount = fileCount,FolderCount=folderCount });
        }

        static void ExportToExcel<T>(IEnumerable<T> list, string path)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                // Add a new worksheet to the Excel file
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Write the header row
                var properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    var cell = worksheet.Cells[1, i + 1];
                    cell.Value = properties[i].Name;

                    // Apply header styles
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(173, 216, 230)); // Background color (Light Blue)
                    cell.Style.Font.Size = 12; // Font size
                    cell.Style.Font.Name = "Calibri"; // Font type
                    cell.Style.Font.Color.SetColor(System.Drawing.Color.FromArgb(0, 0, 0)); // Font color (Black)
                    cell.Style.Font.Bold = true; // Bold font
                }

                // Write the data rows
                int row = 2;
                foreach (var item in list)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var property = properties[i];
                        var value = property.GetValue(item);

                        if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                        {
                            var cell = worksheet.Cells[row, i + 1];
                            cell.Value = value;
                            cell.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss"; // Date and time format
                        }
                        else
                        {
                            worksheet.Cells[row, i + 1].Value = value;
                        }
                    }
                    row++;
                }

                // Save the Excel file
                File.WriteAllBytes(path, package.GetAsByteArray());
            }
        }
    }
}
