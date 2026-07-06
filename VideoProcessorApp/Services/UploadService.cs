namespace VideoProcessorApp.Services
{
    public class UploadService
    {
        public async Task UploadFiles(Action<string> log)
        {
            string outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");
            if (!Directory.Exists(outputDir))
            {
                log("Output folder not found. Run Split first.");
                return;
            }

            string[] files = Directory.GetFiles(outputDir, "*.*", SearchOption.AllDirectories);
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("AccessKey", "your-key");

            string baseUrl = "https://storage.bunnycdn.com/your-storage/";

            foreach (string file in files)
            {
                string relativePath = Path.GetRelativePath(outputDir, file).Replace('\\', '/');
                string url = baseUrl + relativePath;
                log($"Uploading {relativePath}");

                byte[] bytes = await File.ReadAllBytesAsync(file);
                using var content = new ByteArrayContent(bytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                var response = await client.PutAsync(url, content);
                log(response.IsSuccessStatusCode
                    ? $"Uploaded: {relativePath}"
                    : $"FAILED: {relativePath} - {response.StatusCode}");
            }
        }
    }
}