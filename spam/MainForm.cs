using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
        AzureMachineLearningClassifier ws;
        ClassifierValidator cv;

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
                string endpoint = ConfigurationManager.AppSettings["AzureEndpoint"];

                ws = new AzureMachineLearningClassifier(apikey, endpoint);
                ws.OnMessagesClassified += Ws_OnMessagesClassified;

                cv = new ClassifierValidator(ws);
                cv.OnClassifierScored += Cv_OnClassifierScored;
            }
        }

        private void btnClassify_Click(object sender, EventArgs e)
        {
            ws.ClassifyMessageAsync(rtbMessage.Text);

            btnClassify.Enabled = false;
            lblTime.Text = "-";
        }

        private void Ws_OnMessagesClassified(object sender, List<ClassificationResult> e)
        {
            if (e.Count == 1)
            {
                lblClassification.Text = e[0].Classification;
                lblTime.Text = (int)e[0].ElapsedTime.TotalMilliseconds + "ms";
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
                    cv.ScoreClassifierAsync(ofdMessages.FileName);

                    btnClassifyFile.Enabled = false;
                    rtbScore.Text = "Scoring classifier...";
                }
            }
        }

        private void Cv_OnClassifierScored(object sender, ClassifierValidationResult e)
        {
            rtbScore.Text += "\r\nMessages: " + e.Total;
            rtbScore.Text += "\r\nCorrect: " + e.Correct;
            rtbScore.Text += "\r\nIncorrect: " + e.Wrong;
            rtbScore.Text += "\r\nAccuracy: " + (e.Accuracy * 100).ToString("0.00") + "%";
            rtbScore.Text += "\r\nTime: " + (int)e.ElapsedTime.TotalMilliseconds + "ms";
            rtbScore.Text += "\r\nResults saved to " + sfdClassified.FileName;

            using (StreamWriter w = new StreamWriter(sfdClassified.FileName))
            {
                w.WriteLine("Correct,Guess,Message");

                foreach (LabeledMessage message in e.LabeledMessages)
                {
                    w.WriteLine(message.RealClassification + "," + message.ModelClassification + "," + message.Message);
                }
            }

            btnClassifyFile.Enabled = true;
        }
    }
}
