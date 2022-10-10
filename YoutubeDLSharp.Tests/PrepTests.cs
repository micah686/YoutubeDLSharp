using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YoutubeDLSharp.Tests
{
    internal static class PrepTests
    {
        private static bool _didDownloadBinaries = false;

        internal static void DownloadBinaries()
        {
            if (_didDownloadBinaries == false)
            {
                if (!File.Exists("yt-dlp.exe"))
                {
                    YtDlp.DownloadYtDlpBinary();
                }
                if (!File.Exists("ffmpeg.exe"))
                {
                    YtDlp.DownloadFFmpegBinary();
                }
                _didDownloadBinaries = true;
            }
        }
    }
}