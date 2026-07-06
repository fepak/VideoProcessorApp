using VideoProcessorApp.Services;
using System.Text.RegularExpressions;

namespace VideoProcessorApp
{
    public partial class Form1 : Form
    {
        private readonly YoutubeService youtubeService = new YoutubeService();
        private readonly FfmpegService ffmpegService = new FfmpegService();
        private readonly UploadService uploadService = new UploadService();

        private string _videoTitle = "";
        private string _videoFilePath = "";
        private double _videoDurationSec = 0;
        private long _videoFileSize = 0;
        private bool _localMode = false;

        public Form1()
        {
            InitializeComponent();

            ApplyButtonHover(btnDownload);
            ApplyButtonHover(btnSplit);
            ApplyButtonHover(btnUpload);
            ApplyButtonHover(btnLoadQualities);
            ApplyButtonHover(btnPreviewSplit);
            ApplyButtonHover(btnBrowseLocal);
        }

        private void Log(string text)
        {
            if (txtLog.InvokeRequired)
                txtLog.Invoke(() => txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}{Environment.NewLine}"));
            else
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}{Environment.NewLine}");
                txtLog.ScrollToCaret();
            }
        }

        private void SetProgress(int value)
        {
            if (progressBar1.InvokeRequired)
                progressBar1.Invoke(() => progressBar1.Value = value);
            else
                progressBar1.Value = value;
        }

        // ---------- Mode switching ----------
        private void chkLocalFile_CheckedChanged(object sender, EventArgs e)
        {
            _localMode = chkLocalFile.Checked;

            // YouTube controls
            txtUrl.Enabled = !_localMode;
            btnLoadQualities.Enabled = !_localMode;
            cmbResolution.Enabled = !_localMode && cmbResolution.Items.Count > 0;
            cmbFormat.Enabled = !_localMode && cmbResolution.Items.Count > 0;
            btnDownload.Enabled = !_localMode;

            // Local controls
            btnBrowseLocal.Visible = _localMode;
            lblLocalFilePath.Visible = _localMode;

            if (_localMode)
            {
                // Clear YouTube-related internal state
                _videoTitle = "";
                _videoFilePath = "";
                _videoDurationSec = 0;
                _videoFileSize = 0;
                lblVideoInfo.Text = "";
                numParts.Enabled = false;
                btnPreviewSplit.Enabled = false;
                btnSplit.Enabled = false;
                Log("Local file mode selected. Click Browse... to choose a video.");
            }
            else
            {
                // Switch back to YouTube mode
                cmbResolution.Items.Clear();
                cmbResolution.Enabled = false;
                cmbFormat.Enabled = false;
                lblLocalFilePath.Text = "";
                numParts.Enabled = false;
                btnPreviewSplit.Enabled = false;
                btnSplit.Enabled = false;
                Log("YouTube mode selected.");
            }
        }

        private async void btnBrowseLocal_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select a video file";
            ofd.Filter = "Video Files|*.mp4;*.avi;*.webm;*.mkv;*.mov;*.flv;*.wmv|All Files|*.*";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string path = ofd.FileName;
            try
            {
                // Extract title from filename without extension
                _videoTitle = Path.GetFileNameWithoutExtension(path);
                _videoFilePath = path;
                lblLocalFilePath.Text = path;

                // Get duration and file size
                var fileInfo = new FileInfo(path);
                _videoFileSize = fileInfo.Length;
                _videoDurationSec = await ffmpegService.GetDuration(path);

                string container = Path.GetExtension(path).TrimStart('.').ToLower();
                string sanitized = SanitizeFileName(_videoTitle);
                lblVideoInfo.Text = $"{sanitized}.{container}  |  Duration: {TimeSpan.FromSeconds(_videoDurationSec):hh\\:mm\\:ss}  |  Size: {FormatSize(_videoFileSize)}";
                Log($"Local video loaded: {path}");
                Log($"File: {sanitized}.{container}  Duration: {_videoDurationSec:F2}s  Size: {FormatSize(_videoFileSize)}");

                // Enable split controls
                numParts.Enabled = true;
                btnPreviewSplit.Enabled = true;
                btnSplit.Enabled = true;
            }
            catch (Exception ex)
            {
                Log($"ERROR: Could not read video: {ex.Message}");
                _videoFilePath = "";
            }
        }

        // ---------- YouTube actions (unchanged except for small logic) ----------
        private async void btnLoadQualities_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                Log("Paste a YouTube URL first.");
                return;
            }

            try
            {
                ToggleButtons(false);
                cmbResolution.Items.Clear();
                Log("Loading title and available heights...");

                _videoTitle = await youtubeService.GetVideoTitle(txtUrl.Text);
                Log($"Title: {_videoTitle}");

                var heights = await youtubeService.GetAvailableHeights(txtUrl.Text);
                if (heights.Count == 0)
                {
                    Log("No video heights found (≥144p). Try another link.");
                    return;
                }

                foreach (int h in heights.OrderByDescending(h => h))
                    cmbResolution.Items.Add($"{h}p");

                cmbResolution.Enabled = true;
                cmbResolution.SelectedIndex = 0;
                cmbFormat.Enabled = true;
                cmbFormat.SelectedIndex = 0;

                Log($"Available: {string.Join(", ", heights.Select(h => $"{h}p"))}");
            }
            catch (Exception ex)
            {
                Log($"ERROR: {ex.Message}");
            }
            finally
            {
                ToggleButtons(true);
            }
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                Log("Paste a YouTube URL first.");
                return;
            }
            if (cmbResolution.SelectedItem == null || cmbFormat.SelectedItem == null)
            {
                Log("Load qualities and select resolution/format first.");
                return;
            }

            try
            {
                ToggleButtons(false);
                SetProgress(10);

                string resolutionStr = cmbResolution.SelectedItem.ToString();
                string heightStr = resolutionStr.TrimEnd('p');
                if (!int.TryParse(heightStr, out int height))
                    height = 0;

                string container = cmbFormat.SelectedItem.ToString().ToLower();
                Log($"Downloading {height}p ({container})...");

                string sanitized = SanitizeFileName(_videoTitle);
                if (string.IsNullOrWhiteSpace(sanitized)) sanitized = "video";

                string outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Videos");
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, $"{sanitized}.{container}");
                _videoFilePath = outputPath;

                await youtubeService.DownloadVideo(txtUrl.Text, height, container, outputPath, Log);
                SetProgress(40);
                Log("Download complete.");

                var fileInfo = new FileInfo(outputPath);
                _videoFileSize = fileInfo.Length;
                _videoDurationSec = await ffmpegService.GetDuration(outputPath);

                lblVideoInfo.Text = $"{sanitized}.{container}  |  Duration: {TimeSpan.FromSeconds(_videoDurationSec):hh\\:mm\\:ss}  |  Size: {FormatSize(_videoFileSize)}";
                Log($"File: {sanitized}.{container}  Duration: {_videoDurationSec:F2}s  Size: {FormatSize(_videoFileSize)}");

                numParts.Enabled = true;
                btnPreviewSplit.Enabled = true;
                btnSplit.Enabled = true;
                SetProgress(100);
            }
            catch (Exception ex)
            {
                Log($"ERROR: {ex.Message}");
            }
            finally
            {
                ToggleButtons(true);
            }
        }

        // ---------- Split / Preview / Upload (unchanged) ----------
        private void btnPreviewSplit_Click(object sender, EventArgs e)
        {
            if (_videoDurationSec <= 0 || _videoFileSize == 0)
            {
                Log("No video loaded. Download or select a local file first.");
                return;
            }

            int parts = (int)numParts.Value;
            if (parts < 1) parts = 1;

            double segmentDuration = _videoDurationSec / parts;
            string sanitized = SanitizeFileName(_videoTitle);
            if (string.IsNullOrWhiteSpace(sanitized)) sanitized = "video";

            Log("\n--------- PREDICTED SPLIT ---------");
            Log($"{"#",-4}{"Start",-15}{"End",-15}{"Duration",-15}{"File size (est.)",-18}{"Output"}");

            for (int i = 0; i < parts; i++)
            {
                double start = i * segmentDuration;
                double end = (i == parts - 1) ? _videoDurationSec : (i + 1) * segmentDuration;
                double partDuration = end - start;
                long partSize = (long)(_videoFileSize * (partDuration / _videoDurationSec));

                string startStr = TimeSpan.FromSeconds(start).ToString(@"hh\:mm\:ss\.fff");
                string endStr = TimeSpan.FromSeconds(end).ToString(@"hh\:mm\:ss\.fff");
                string durStr = TimeSpan.FromSeconds(partDuration).ToString(@"hh\:mm\:ss\.fff");
                string sizeStr = FormatSize(partSize);
                string outName = $"{sanitized}/part_{i:D3}.mp4";

                Log($"{i + 1,-4}{startStr,-15}{endStr,-15}{durStr,-15}{sizeStr,-18}{outName}");
            }
            Log("-----------------------------------\n");
        }

        private async void btnSplit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_videoFilePath) || !File.Exists(_videoFilePath))
            {
                Log("No video file available. Load a video first.");
                return;
            }
            if (_videoDurationSec <= 0)
            {
                Log("Cannot determine video duration.");
                return;
            }

            int parts = (int)numParts.Value;
            if (parts < 1) parts = 1;

            try
            {
                ToggleButtons(false);
                SetProgress(50);
                Log($"Splitting into {parts} parts...");

                string sanitized = SanitizeFileName(_videoTitle);
                if (string.IsNullOrWhiteSpace(sanitized)) sanitized = "video";
                string outputSubDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output", sanitized);
                Directory.CreateDirectory(outputSubDir);

                await ffmpegService.SplitVideo(_videoFilePath, parts, _videoDurationSec, outputSubDir, Log);
                SetProgress(80);
                Log("Split complete.");
            }
            catch (Exception ex)
            {
                Log($"ERROR: {ex.Message}");
            }
            finally
            {
                ToggleButtons(true);
            }
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                ToggleButtons(false);
                SetProgress(85);
                Log("Uploading files...");
                await uploadService.UploadFiles(Log);
                SetProgress(100);
                Log("Upload complete.");
            }
            catch (Exception ex)
            {
                Log($"ERROR: {ex.Message}");
            }
            finally
            {
                ToggleButtons(true);
            }
        }

        // ---------- Helpers ----------
        private void ToggleButtons(bool enabled)
        {
            btnDownload.Enabled = enabled && !_localMode;
            btnSplit.Enabled = enabled && !string.IsNullOrEmpty(_videoFilePath);
            btnUpload.Enabled = enabled;
            btnLoadQualities.Enabled = enabled && !_localMode;
            btnPreviewSplit.Enabled = enabled && !string.IsNullOrEmpty(_videoFilePath);
            numParts.Enabled = enabled && !string.IsNullOrEmpty(_videoFilePath);
            btnBrowseLocal.Enabled = enabled;
        }

        private void ApplyButtonHover(Button btn)
        {
            Color original = btn.BackColor;
            btn.MouseEnter += (s, e) => { if (btn.Enabled) btn.BackColor = Color.FromArgb(65, 65, 65); };
            btn.MouseLeave += (s, e) => { if (btn.Enabled) btn.BackColor = original; };
        }

        public static string SanitizeFileName(string name, int maxLength = 80)
        {
            if (string.IsNullOrWhiteSpace(name)) return "video";
            char[] invalid = Path.GetInvalidFileNameChars();
            string sanitized = string.Concat(name.Select(c => invalid.Contains(c) ? '_' : c));
            sanitized = Regex.Replace(sanitized, @"_+", "_").Trim('_');
            if (sanitized.Length > maxLength)
                sanitized = sanitized[..maxLength].TrimEnd('_');
            return string.IsNullOrWhiteSpace(sanitized) ? "video" : sanitized;
        }

        public static string FormatSize(long bytes)
        {
            double mb = bytes / (1024.0 * 1024.0);
            return $"{mb:F2}M";
        }
    }
}