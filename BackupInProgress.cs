using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AzureBlob_Download
{
    public partial class BackupInProgress : Form
    {
        public ProgressBar progressBar
        {
            get { return progressBar1; }
        }
        public BackupInProgress()
        {
            InitializeComponent();
        }

        

        public void SetMyProgressBarValue(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action<int>(SetMyProgressBarValue), value);
            }
            else
            {
                progressBar1.Value = value;
            }
        }

        public void IncrementMyProgressBarValue(int increment)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new Action<int>(IncrementMyProgressBarValue), increment);
            }
            else
            {
                progressBar1.Increment(increment);
            }
        }

        public Label MyLabel
        {
            get { return label1; }
        }

        // Or create a public method to interact with the TextBox control
        public void SetMyTextBoxText(string text)
        {
            

            if (label1.InvokeRequired)
            {
                label1.Invoke(new Action<string>(SetMyTextBoxText), text);
            }
            else
            {
                label1.Text = text;
            }
        }

        public void ChangeColor(Color colour)
        {
            
            if (label1.InvokeRequired)
            {
                label1.Invoke(new Action<Color>(ChangeColor), colour);
            }
            else
            {
                label1.ForeColor = colour;
            }

        }

        public void SetMyTextBoxText1(string text)
        {


            if (label2.InvokeRequired)
            {
                label2.Invoke(new Action<string>(SetMyTextBoxText1), text);
            }
            else
            {
                label2.Text = text;
            }
        }

        public void ChangeColor1(Color colour)
        {

            if (label2.InvokeRequired)
            {
                label2.Invoke(new Action<Color>(ChangeColor1), Color.Green);
            }
            else
            {
                label2.ForeColor = Color.Green;
            }

        }

    }
}
