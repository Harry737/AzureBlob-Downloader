using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AzureBlob_Download
{
    public partial class BlobInProgress : Form
    {
        public BlobInProgress()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
            string connectionString= textBox1.Text;
            string Path=textBox2.Text;
            this.Hide();
            BackupInProgress form = new BackupInProgress();
            form.Show();
            Form3 status = new Form3();
            Downloader ds = new Downloader(status);
            Downloader d = new Downloader(form);
            bool isSuccess=await Task.Run(() => Downloader.Download(connectionString, Path));
            if (isSuccess)
            {
                form.Hide();
                status.Show();
            }
            


        }

    }
}
