using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spam
{
    public partial class MainForm : Form
    {
        AzureWS ws;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!File.Exists("api.key"))
            {
                ApiKeyForm keyForm = new ApiKeyForm();
                if (keyForm.ShowDialog() != DialogResult.OK || !File.Exists("api.key"))
                {
                    MessageBox.Show("You need to set up an Azure API key and web service first!", "Spam ML", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }
            else
            {
                string apikey = File.ReadAllText("api.key");

                ws = new AzureWS(apikey);
                ws.OnMessageClassified += OnMessageClassified;
            }
        }

        private void btnClassify_Click(object sender, EventArgs e)
        {
            ws.ClassifyMesageAsync(rtbMessage.Text);

            btnClassify.Enabled = false;
            lblTime.Text = "-";
        }

        private void OnMessageClassified(object sender, ClassificationResult result)
        {
            if (result != null)
            {
                lblClassification.Text = result.Classification;
                lblTime.Text = (int)result.ElapsedTime.TotalMilliseconds + "ms";
            }
            else
            {
                lblClassification.Text = "Oops!";
            }

            btnClassify.Enabled = true;
        }

        private void btnClassifyFile_Click(object sender, EventArgs e)
        {
            if (ofdMessages.ShowDialog() == DialogResult.OK)
            {
                if (sfdClassified.ShowDialog() == DialogResult.OK)
                {
                    string[] messages = File.ReadAllLines(ofdMessages.FileName);
                    using (StreamWriter w = new StreamWriter(sfdClassified.FileName))
                    {
                        foreach (string message in messages)
                        {
                            if (message.StartsWith("Ham") || message.StartsWith("Spam"))
                            {
                                // TODO: run all messages through the service, compare results to actual labels, and clean up the UI
                            }
                        }
                    }
                }
            }
        }
    }
}
