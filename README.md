# Video Processor (YouTube Downloader, Splitter, Cloud Uploader)

Desktop application on Windows Forms (C#) that automates three frequent tasks:
- Downloading videos from YouTube with a choice of quality and format,
- Splitting any video into equal parts without re-encoding,
- Batch uploading of resulting files to cloud storage (BunnyCDN / compatible API).

The tool is created as a replacement for manual work with console utilities: now the whole pipeline is launched with a couple of clicks.

## Features
- YouTube downloading using yt-dlp - choose resolution (144p...4K) and container (MP4, AVI, WebM)
- Local mode - work with any video file on disk (mp4, mkv, webm, etc.)
- Splitting into parts with preview of duration and size
- Cloud upload - sends all files from the output folder to the specified HTTP endpoint (PUT requests with AccessKey)
- Separation of concerns - services YoutubeService, FfmpegService, UploadService are easy to test and replace

## Technologies and Dependencies
- .NET 8 (WinForms, C#)
- [yt-dlp](https://github.com/yt-dlp/yt-dlp) - universal video downloader
- [FFmpeg](https://ffmpeg.org/) + ffprobe - video processing and duration retrieval
- BunnyCDN (optional) - or any storage with compatible REST API

## Requirements
- Windows 10/11 (x64)
- [.NET 8 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (or include the runtime in the publication)
- Installed (or portable):
  - `yt-dlp.exe` - download from the [official repository](https://github.com/yt-dlp/yt-dlp/releases)
  - `ffmpeg.exe` and `ffprobe.exe` - e.g., from the [official FFmpeg website](https://ffmpeg.org/download.html)

## Quick Start

1. **Specify paths to utilities**  
   In `YoutubeService.cs` and `FfmpegService.cs` change the constants:

   private readonly string ytDlpPath = @"C:\path\to\yt-dlp.exe";
   private readonly string ffmpegPath = @"C:\path\to\ffmpeg\bin\ffmpeg.exe";
   private readonly string ffprobePath = @"C:\path\to\ffmpeg\bin\ffprobe.exe";
   
2. **Configure cloud storage (optional)**
	In UploadService.cs replace the AccessKey and base URL with your own:

	client.DefaultRequestHeaders.Add("AccessKey", "your-key");
	string baseUrl = "https://storage.bunnycdn.com/your-storage/";

3. **Build the project**
	Open VideoProcessorApp.sln in Visual Studio 2022 (or newer).
	Build the solution (Build -> Build Solution).
	Run the executable or press F5.

4. **Use the program**
	Paste a YouTube link, click Load Qualities -> select resolution and format -> Download.
	Or check Use local video file and choose a file on disk.
	Set the number of parts and click Split (you can press Preview Split before splitting to see estimated times and sizes).
	To upload to the cloud, click Upload.
	
##License
Distributed under the MIT License. See LICENSE file for details.

##Important
Downloading videos from YouTube may violate the platform's terms of service. Use the application only for downloading your own content or for educational purposes.
When splitting with stream copy (-c copy), segment durations will be approximate - FFmpeg cuts at the nearest keyframe.