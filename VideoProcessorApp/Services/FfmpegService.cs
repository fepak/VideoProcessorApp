using VideoProcessorApp.Helpers;
using System.Globalization;

namespace VideoProcessorApp.Services
{
    public class FfmpegService
    {
        private readonly string ffmpegPath = @"C:\E\wwk\ffmpeg\bin\ffmpeg.exe";
        private readonly string ffprobePath = @"C:\E\wwk\ffmpeg\bin\ffprobe.exe";

        public async Task<double> GetDuration(string videoPath)
        {
            string args = $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{videoPath}\"";
            string result = "";
            await ProcessHelper.RunProcess(ffprobePath, args, line =>
            {
                if (!string.IsNullOrWhiteSpace(line)) result += line;
            });
            if (double.TryParse(result.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double sec))
                return sec;
            throw new Exception("Failed to get video duration.");
        }

        public async Task SplitVideo(string inputPath, int parts, double totalDuration, string outputDir, Action<string> log)
        {
            double segmentTime = totalDuration / parts;
            string segmentTimeStr = segmentTime.ToString("F6", CultureInfo.InvariantCulture);
            string outputPattern = Path.Combine(outputDir, "part_%03d.mp4");

            string args = $"-i \"{inputPath}\" -f segment -segment_time {segmentTimeStr} -c copy -reset_timestamps 1 \"{outputPattern}\"";
            await ProcessHelper.RunProcess(ffmpegPath, args, log);
        }
    }
}