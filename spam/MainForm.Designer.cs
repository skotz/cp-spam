namespace spam
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.btnClassify = new System.Windows.Forms.Button();
            this.lblClassification = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.ofdMessages = new System.Windows.Forms.OpenFileDialog();
            this.sfdClassified = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.rtbScore = new System.Windows.Forms.RichTextBox();
            this.btnClassifyFile = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.rtbScoreWord = new System.Windows.Forms.RichTextBox();
            this.btnWord = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ofdTrainingData = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbMessage
            // 
            this.rtbMessage.Location = new System.Drawing.Point(6, 6);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(585, 322);
            this.rtbMessage.TabIndex = 0;
            this.rtbMessage.Text = resources.GetString("rtbMessage.Text");
            // 
            // btnClassify
            // 
            this.btnClassify.Location = new System.Drawing.Point(500, 334);
            this.btnClassify.Name = "btnClassify";
            this.btnClassify.Size = new System.Drawing.Size(91, 23);
            this.btnClassify.TabIndex = 1;
            this.btnClassify.Text = "Classify";
            this.btnClassify.UseVisualStyleBackColor = true;
            this.btnClassify.Click += new System.EventHandler(this.btnClassify_Click);
            // 
            // lblClassification
            // 
            this.lblClassification.AutoSize = true;
            this.lblClassification.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClassification.ForeColor = System.Drawing.Color.Green;
            this.lblClassification.Location = new System.Drawing.Point(6, 331);
            this.lblClassification.Name = "lblClassification";
            this.lblClassification.Size = new System.Drawing.Size(122, 24);
            this.lblClassification.TabIndex = 2;
            this.lblClassification.Text = "Unclassified";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.Silver;
            this.lblTime.Location = new System.Drawing.Point(162, 331);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(17, 24);
            this.lblTime.TabIndex = 3;
            this.lblTime.Text = "-";
            // 
            // ofdMessages
            // 
            this.ofdMessages.FileName = "SpamDetectionData.txt";
            // 
            // sfdClassified
            // 
            this.sfdClassified.FileName = "classified.csv";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(604, 388);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rtbMessage);
            this.tabPage1.Controls.Add(this.lblTime);
            this.tabPage1.Controls.Add(this.btnClassify);
            this.tabPage1.Controls.Add(this.lblClassification);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(596, 362);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Classify Azure Message";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.rtbScore);
            this.tabPage2.Controls.Add(this.btnClassifyFile);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(596, 362);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Score Azure Model";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // rtbScore
            // 
            this.rtbScore.Location = new System.Drawing.Point(6, 6);
            this.rtbScore.Name = "rtbScore";
            this.rtbScore.Size = new System.Drawing.Size(584, 319);
            this.rtbScore.TabIndex = 3;
            this.rtbScore.Text = "Click \"begin\" to...\n   1) Select a labeled dataset file to score the tained model" +
    " on\n   2) Select a location to save the results";
            // 
            // btnClassifyFile
            // 
            this.btnClassifyFile.Location = new System.Drawing.Point(499, 331);
            this.btnClassifyFile.Name = "btnClassifyFile";
            this.btnClassifyFile.Size = new System.Drawing.Size(91, 23);
            this.btnClassifyFile.TabIndex = 2;
            this.btnClassifyFile.Text = "Score Model";
            this.btnClassifyFile.UseVisualStyleBackColor = true;
            this.btnClassifyFile.Click += new System.EventHandler(this.btnClassifyFile_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.rtbScoreWord);
            this.tabPage3.Controls.Add(this.btnWord);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(596, 362);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Score Word Model";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // rtbScoreWord
            // 
            this.rtbScoreWord.Location = new System.Drawing.Point(8, 25);
            this.rtbScoreWord.Name = "rtbScoreWord";
            this.rtbScoreWord.Size = new System.Drawing.Size(580, 300);
            this.rtbScoreWord.TabIndex = 2;
            this.rtbScoreWord.Text = "Click \"begin\" to...\n   1) Select a labeled dataset file to train on\n   2) Select " +
    "a labeled dataset file to score the trained model on";
            // 
            // btnWord
            // 
            this.btnWord.Location = new System.Drawing.Point(513, 331);
            this.btnWord.Name = "btnWord";
            this.btnWord.Size = new System.Drawing.Size(75, 23);
            this.btnWord.TabIndex = 1;
            this.btnWord.Text = "Begin";
            this.btnWord.UseVisualStyleBackColor = true;
            this.btnWord.Click += new System.EventHandler(this.btnWord_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This is a very basic word frequency classifier.";
            // 
            // ofdTrainingData
            // 
            this.ofdTrainingData.FileName = "train.csv";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 388);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "Spam ML";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.Button btnClassify;
        private System.Windows.Forms.Label lblClassification;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.OpenFileDialog ofdMessages;
        private System.Windows.Forms.SaveFileDialog sfdClassified;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnClassifyFile;
        private System.Windows.Forms.RichTextBox rtbScore;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnWord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rtbScoreWord;
        private System.Windows.Forms.OpenFileDialog ofdTrainingData;
    }
}

