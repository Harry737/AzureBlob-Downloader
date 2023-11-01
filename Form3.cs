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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public Label MyLabel
        {
            get { return label4; }
        }

        // Or create a public method to interact with the TextBox control
        public void SetMyTextBoxText(string text)
        {


            if (label4.InvokeRequired)
            {
                label4.Invoke(new Action<string>(SetMyTextBoxText), text);
            }
            else
            {
                label4.Text = text;
            }
        }

        public Label MyLabel2
        {
            get { return label5; }
        }

        // Or create a public method to interact with the TextBox control
        public void SetMyTextBoxText2(string text)
        {


            if (label5.InvokeRequired)
            {
                label5.Invoke(new Action<string>(SetMyTextBoxText2), text);
            }
            else
            {
                label5.Text = text;
            }
        }

        public Label MyLabel3
        {
            get { return label6; }
        }

        // Or create a public method to interact with the TextBox control
        public void SetMyTextBoxText3(string text)
        {


            if (label6.InvokeRequired)
            {
                label6.Invoke(new Action<string>(SetMyTextBoxText3), text);
            }
            else
            {
                label6.Text = text;
            }
        }
    }
}
