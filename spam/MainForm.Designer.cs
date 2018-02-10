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
            this.btnClassifyFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtbMessage
            // 
            this.rtbMessage.Location = new System.Drawing.Point(12, 12);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.Size = new System.Drawing.Size(585, 322);
            this.rtbMessage.TabIndex = 0;
            this.rtbMessage.Text = resources.GetString("rtbMessage.Text");
            // 
            // btnClassify
            // 
            this.btnClassify.Location = new System.Drawing.Point(506, 340);
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
            this.lblClassification.Location = new System.Drawing.Point(12, 337);
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
            this.lblTime.Location = new System.Drawing.Point(168, 337);
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
            // btnClassifyFile
            // 
            this.btnClassifyFile.Location = new System.Drawing.Point(409, 340);
            this.btnClassifyFile.Name = "btnClassifyFile";
            this.btnClassifyFile.Size = new System.Drawing.Size(91, 23);
            this.btnClassifyFile.TabIndex = 1;
            this.btnClassifyFile.Text = "Classify";
            this.btnClassifyFile.UseVisualStyleBackColor = true;
            this.btnClassifyFile.Click += new System.EventHandler(this.btnClassifyFile_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 375);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblClassification);
            this.Controls.Add(this.btnClassifyFile);
            this.Controls.Add(this.btnClassify);
            this.Controls.Add(this.rtbMessage);
            this.Name = "MainForm";
            this.Text = "Spam ML";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.Button btnClassify;
        private System.Windows.Forms.Label lblClassification;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.OpenFileDialog ofdMessages;
        private System.Windows.Forms.SaveFileDialog sfdClassified;
        private System.Windows.Forms.Button btnClassifyFile;
    }
}

