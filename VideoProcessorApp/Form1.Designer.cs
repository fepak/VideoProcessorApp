namespace VideoProcessorApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtUrl = new TextBox();
            btnDownload = new Button();
            btnSplit = new Button();
            btnUpload = new Button();
            progressBar1 = new ProgressBar();
            label1 = new Label();
            txtLog = new RichTextBox();
            cmbResolution = new ComboBox();
            btnLoadQualities = new Button();
            lblResolution = new Label();
            lblFormat = new Label();
            cmbFormat = new ComboBox();
            lblParts = new Label();
            numParts = new NumericUpDown();
            btnPreviewSplit = new Button();
            lblVideoInfo = new Label();
            chkLocalFile = new CheckBox();
            btnBrowseLocal = new Button();
            lblLocalFilePath = new Label();
            ((System.ComponentModel.ISupportInitialize)numParts).BeginInit();
            SuspendLayout();

            // label1
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(300, 15);
            label1.Name = "label1";
            label1.Size = new Size(290, 45);
            label1.Text = "Video Processor";

            // chkLocalFile
            chkLocalFile.AutoSize = true;
            chkLocalFile.Font = new Font("Segoe UI", 10F);
            chkLocalFile.ForeColor = Color.White;
            chkLocalFile.Location = new Point(180, 65);
            chkLocalFile.Name = "chkLocalFile";
            chkLocalFile.Size = new Size(130, 23);
            chkLocalFile.TabIndex = 0;
            chkLocalFile.Text = "Use local video file";
            chkLocalFile.CheckedChanged += chkLocalFile_CheckedChanged;

            // btnBrowseLocal
            btnBrowseLocal.BackColor = Color.FromArgb(45, 45, 45);
            btnBrowseLocal.FlatAppearance.BorderSize = 0;
            btnBrowseLocal.FlatStyle = FlatStyle.Flat;
            btnBrowseLocal.Font = new Font("Segoe UI Semibold", 9F);
            btnBrowseLocal.ForeColor = Color.White;
            btnBrowseLocal.Location = new Point(320, 63);
            btnBrowseLocal.Name = "btnBrowseLocal";
            btnBrowseLocal.Size = new Size(100, 26);
            btnBrowseLocal.TabIndex = 1;
            btnBrowseLocal.Text = "Browse...";
            btnBrowseLocal.UseVisualStyleBackColor = false;
            btnBrowseLocal.Click += btnBrowseLocal_Click;
            btnBrowseLocal.Visible = false;

            // lblLocalFilePath
            lblLocalFilePath.AutoSize = true;
            lblLocalFilePath.Font = new Font("Segoe UI", 9F);
            lblLocalFilePath.ForeColor = Color.Gray;
            lblLocalFilePath.Location = new Point(430, 67);
            lblLocalFilePath.Name = "lblLocalFilePath";
            lblLocalFilePath.Size = new Size(0, 15);
            lblLocalFilePath.Visible = false;

            // txtUrl
            txtUrl.BackColor = Color.FromArgb(30, 30, 30);
            txtUrl.BorderStyle = BorderStyle.FixedSingle;
            txtUrl.ForeColor = Color.White;
            txtUrl.Font = new Font("Segoe UI", 11F);
            txtUrl.Location = new Point(180, 100);
            txtUrl.Name = "txtUrl";
            txtUrl.PlaceholderText = "Paste YouTube URL...";
            txtUrl.Size = new Size(540, 27);
            txtUrl.TabIndex = 2;

            // btnLoadQualities
            btnLoadQualities.BackColor = Color.FromArgb(45, 45, 45);
            btnLoadQualities.FlatAppearance.BorderSize = 0;
            btnLoadQualities.FlatStyle = FlatStyle.Flat;
            btnLoadQualities.Font = new Font("Segoe UI Semibold", 9F);
            btnLoadQualities.ForeColor = Color.White;
            btnLoadQualities.Location = new Point(580, 138);
            btnLoadQualities.Click += btnLoadQualities_Click;
            btnLoadQualities.Name = "btnLoadQualities";
            btnLoadQualities.Size = new Size(140, 28);
            btnLoadQualities.Text = "Load Qualities";
            btnLoadQualities.UseVisualStyleBackColor = false;

            // lblResolution
            lblResolution.AutoSize = true;
            lblResolution.Font = new Font("Segoe UI", 10F);
            lblResolution.ForeColor = Color.White;
            lblResolution.Location = new Point(180, 143);
            lblResolution.Text = "Resolution:";

            // cmbResolution
            cmbResolution.BackColor = Color.FromArgb(30, 30, 30);
            cmbResolution.ForeColor = Color.White;
            cmbResolution.FlatStyle = FlatStyle.Flat;
            cmbResolution.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbResolution.Location = new Point(275, 140);
            cmbResolution.Size = new Size(100, 25);
            cmbResolution.Enabled = false;

            // lblFormat
            lblFormat.AutoSize = true;
            lblFormat.Font = new Font("Segoe UI", 10F);
            lblFormat.ForeColor = Color.White;
            lblFormat.Location = new Point(385, 143);
            lblFormat.Text = "Format:";

            // cmbFormat
            cmbFormat.BackColor = Color.FromArgb(30, 30, 30);
            cmbFormat.ForeColor = Color.White;
            cmbFormat.FlatStyle = FlatStyle.Flat;
            cmbFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFormat.Location = new Point(455, 140);
            cmbFormat.Size = new Size(100, 25);
            cmbFormat.Enabled = false;
            cmbFormat.Items.AddRange(new[] { "MP4", "AVI", "WEBM" });
            cmbFormat.SelectedIndex = 0;

            // Split settings
            lblParts.AutoSize = true;
            lblParts.Font = new Font("Segoe UI", 10F);
            lblParts.ForeColor = Color.White;
            lblParts.Location = new Point(180, 185);
            lblParts.Text = "Split into parts:";

            numParts.BackColor = Color.FromArgb(30, 30, 30);
            numParts.ForeColor = Color.White;
            numParts.BorderStyle = BorderStyle.FixedSingle;
            numParts.Minimum = 1;
            numParts.Maximum = 99;
            numParts.Value = 3;
            numParts.Location = new Point(315, 183);
            numParts.Size = new Size(60, 25);
            numParts.Enabled = false;

            btnPreviewSplit.BackColor = Color.FromArgb(45, 45, 45);
            btnPreviewSplit.FlatAppearance.BorderSize = 0;
            btnPreviewSplit.FlatStyle = FlatStyle.Flat;
            btnPreviewSplit.Font = new Font("Segoe UI Semibold", 9F);
            btnPreviewSplit.ForeColor = Color.White;
            btnPreviewSplit.Location = new Point(385, 181);
            btnPreviewSplit.Click += btnPreviewSplit_Click;
            btnPreviewSplit.Name = "btnPreviewSplit";
            btnPreviewSplit.Size = new Size(140, 28);
            btnPreviewSplit.Text = "Preview Split";
            btnPreviewSplit.UseVisualStyleBackColor = false;
            btnPreviewSplit.Enabled = false;

            lblVideoInfo.AutoSize = true;
            lblVideoInfo.Font = new Font("Segoe UI", 9F);
            lblVideoInfo.ForeColor = Color.Gray;
            lblVideoInfo.Location = new Point(180, 215);
            lblVideoInfo.Size = new Size(400, 15);
            lblVideoInfo.Text = "";

            // Main buttons
            btnDownload.BackColor = Color.FromArgb(45, 45, 45);
            btnDownload.FlatAppearance.BorderSize = 0;
            btnDownload.FlatStyle = FlatStyle.Flat;
            btnDownload.Font = new Font("Segoe UI Semibold", 10F);
            btnDownload.ForeColor = Color.White;
            btnDownload.Location = new Point(180, 250);
            btnDownload.Click += btnDownload_Click;
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(150, 45);
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = false;

            btnSplit.BackColor = Color.FromArgb(45, 45, 45);
            btnSplit.FlatAppearance.BorderSize = 0;
            btnSplit.FlatStyle = FlatStyle.Flat;
            btnSplit.Font = new Font("Segoe UI Semibold", 10F);
            btnSplit.ForeColor = Color.White;
            btnSplit.Location = new Point(375, 250);
            btnSplit.Click += btnSplit_Click;
            btnSplit.Name = "btnSplit";
            btnSplit.Size = new Size(150, 45);
            btnSplit.Text = "Split";
            btnSplit.UseVisualStyleBackColor = false;

            btnUpload.BackColor = Color.FromArgb(0, 120, 215);
            btnUpload.FlatAppearance.BorderSize = 0;
            btnUpload.FlatStyle = FlatStyle.Flat;
            btnUpload.Font = new Font("Segoe UI Semibold", 10F);
            btnUpload.ForeColor = Color.White;
            btnUpload.Location = new Point(570, 250);
            btnUpload.Click += btnUpload_Click;
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(150, 45);
            btnUpload.Text = "Upload";
            btnUpload.UseVisualStyleBackColor = false;

            // progressBar1
            progressBar1.Location = new Point(180, 315);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(540, 18);
            progressBar1.Style = ProgressBarStyle.Continuous;

            // txtLog
            txtLog.BackColor = Color.FromArgb(24, 24, 24);
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Font = new Font("Consolas", 10F);
            txtLog.ForeColor = Color.LightGray;
            txtLog.Location = new Point(180, 350);
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.Size = new Size(540, 200);

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(18, 18, 18);
            ClientSize = new Size(900, 600);
            Font = new Font("Segoe UI", 10F);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Video Processor";

            Controls.Add(label1);
            Controls.Add(chkLocalFile);
            Controls.Add(btnBrowseLocal);
            Controls.Add(lblLocalFilePath);
            Controls.Add(txtUrl);
            Controls.Add(btnLoadQualities);
            Controls.Add(lblResolution);
            Controls.Add(cmbResolution);
            Controls.Add(lblFormat);
            Controls.Add(cmbFormat);
            Controls.Add(lblParts);
            Controls.Add(numParts);
            Controls.Add(btnPreviewSplit);
            Controls.Add(lblVideoInfo);
            Controls.Add(btnDownload);
            Controls.Add(btnSplit);
            Controls.Add(btnUpload);
            Controls.Add(progressBar1);
            Controls.Add(txtLog);

            ((System.ComponentModel.ISupportInitialize)numParts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox txtUrl;
        private Button btnDownload;
        private Button btnSplit;
        private Button btnUpload;
        private ProgressBar progressBar1;
        private Label label1;
        private RichTextBox txtLog;
        private ComboBox cmbResolution;
        private Button btnLoadQualities;
        private Label lblResolution;
        private Label lblFormat;
        private ComboBox cmbFormat;
        private Label lblParts;
        private NumericUpDown numParts;
        private Button btnPreviewSplit;
        private Label lblVideoInfo;
        private CheckBox chkLocalFile;
        private Button btnBrowseLocal;
        private Label lblLocalFilePath;
    }
}