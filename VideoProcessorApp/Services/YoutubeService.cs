using VideoProcessorApp.Helpers;
using System.Text.RegularExpressions;

namespace VideoProcessorApp.Services
{
    public class YoutubeService
    {
        private readonly string ytDlpPath = @"C:\E\wwk\yt-dlp\yt-dlp.exe";
        private readonly string ffmpegPath = @"C:\E\wwk\ffmpeg\bin\ffmpeg.exe";

        public async Task<string> GetVideoTitle(string url)
        {
            string args = $"--get-title {url}";
            string title = "";
            await ProcessHelper.RunProcess(ytDlpPath, args, line =>
            {
                if (!string.IsNullOrWhiteSpace(line))
                    title = line.Trim();
            });
            return title;
        }

        // Returns unique video heights (144p and above)

        public async Task<HashSet<int>> GetAvailableHeights(string url)
        {
            var heights = new HashSet<int>();
            string args = $"-F {url}";
            bool inFormats = false;

            await ProcessHelper.RunProcess(ytDlpPath, args, line =>
            {
                if (string.IsNullOrWhiteSpace(line)) return;
                if (!inFormats && line.Contains("ID") && line.Contains("EXT")) { inFormats = true; return; }
                if (!inFormats) return;

                Match resMatch = Regex.Match(line, @"(\d{2,4})x(\d{2,4})");
                if (resMatch.Success)
                {
                    if (int.TryParse(resMatch.Groups[2].Value, out int height) && height >= 144)
                        heights.Add(height);
                }
            });

            heights.Remove(0);
            return heights;
        }

        // Downloads video+audio, merged, in chosen height and container.

        public async Task DownloadVideo(string url, int height, string container, string outputPath, Action<string> log)
        {
            string format;
            if (height <= 0)
            {
                format = $"best[ext={container}]/bestvideo[ext={container}]+bestaudio[ext=m4a]/best";
            }
            else
            {
                format = $"best[height<={height}][ext={container}]/bestvideo[height<={height}][ext={container}]+bestaudio[ext=m4a]/best[height<={height}]";
            }

            // Tell yt-dlp exactly where ffmpeg is so it can merge audio+video
            string args = $"--ffmpeg-location \"{ffmpegPath}\" -f \"{format}\" -o \"{outputPath}\" {url}";
            await ProcessHelper.RunProcess(ytDlpPath, args, log);
        }
    }
}