using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoutubeDLSharp.Helpers;

namespace YoutubeDLSharp
{
    /// <summary>
    /// Utility methods.
    /// </summary>
    public static class Utils
    {
        private static readonly Regex rgxTimestamp = new Regex("[0-9]+(?::[0-9]+)+", RegexOptions.Compiled);
        private static readonly Dictionary<char, string> accentChars
            = "ÂÃÄÀÁÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖŐØŒÙÚÛÜŰÝÞßàáâãäåæçèéêëìíîïðñòóôõöőøœùúûüűýþÿ"
                .Zip(new[] { "A","A","A","A","A","A","AE","C","E","E","E","E","I","I","I","I","D","N",
                    "O","O","O","O","O","O","O","OE","U","U","U","U","U","Y","P","ss",
                    "a","a","a","a","a","a","ae","c","e","e","e","e","i","i","i","i","o","n",
                    "o","o","o","o","o","o","o","oe","u","u","u","u","u","y","p","y"},
                    (c, s) => new { Key = c, Val = s }).ToDictionary(o => o.Key, o => o.Val);

        /// <summary>
        /// Sanitize a string to be a valid file name.
        /// Ported from:
        /// https://github.com/ytdl-org/youtube-dl/blob/33c1c7d80fd99024879a5f087b55b24374385e43/youtube_dl/utils.py#L2067
        /// </summary>
        /// <returns></returns>
        public static string Sanitize(string s, bool restricted = false)
        {
            rgxTimestamp.Replace(s, m => m.Groups[0].Value.Replace(':', '_'));
            var result = string.Join("", s.Select(c => sanitizeChar(c, restricted)));
            result = result.Replace("__", "_").Trim('_');
            if (restricted && result.StartsWith("-_"))
                result = result.Substring(2);
            if (result.StartsWith("-"))
                result = "_" + result.Substring(1);
            result = result.TrimStart('.');
            if (string.IsNullOrWhiteSpace(result))
                result = "_";
            return result;
        }

        private static string sanitizeChar(char c, bool restricted)
        {
            const char CONTROL_CHARS = (char)31;
            const char MAX_CHAR_DEL = (char)127;
            if (restricted && accentChars.ContainsKey(c))
                return accentChars[c];
            else if (c == '?' || c <= CONTROL_CHARS || c == MAX_CHAR_DEL)
                return "";
            else if (c == '"')
                return restricted ? "" : "\'";
            else if (c == ':')
                return restricted ? "_-" : " -";
            else if ("\\/|*<>".Contains(c))
                return "_";
            else if (restricted && "!&\'()[]{}$;`^,# ".Contains(c))
                return "_";
            else if (restricted && c > MAX_CHAR_DEL)
                return "_";
            else return c.ToString();
        }

        /// <summary>
        /// Returns the absolute path for the specified path string.
        /// Also searches the environment's PATH variable.
        /// </summary>
        /// <param name="fileName">The relative path string.</param>
        /// <returns>The absolute path or null if the file was not found.</returns>
        public static string GetFullPath(string fileName)
        {
            if (File.Exists(fileName))
                return Path.GetFullPath(fileName);

            var values = Environment.GetEnvironmentVariable("PATH");
            foreach (var p in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(p, fileName);
                if (File.Exists(fullPath))
                    return fullPath;
            }
            return null;
        }

        /// <summary>
        /// Downloads the YT-DLP binary depending on OS
        /// </summary>
        /// <param name="directoryPath">The optional directory of where it should be saved to</param>
        /// <exception cref="Exception"></exception>
        public static void DownloadYtDlp(string directoryPath = "")
        {
            const string BASE_GITHUB_URL = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp";
            string downloadUrl = OSHelper.GetOSVersion() switch
            {
                OSVersion.Windows => $"{BASE_GITHUB_URL}.exe",
                OSVersion.OSX => $"{BASE_GITHUB_URL}_macos",
                OSVersion.Linux => BASE_GITHUB_URL,
                _ => throw new Exception("Your OS isn't supported")
            };

            if (string.IsNullOrEmpty(directoryPath)) { directoryPath = Directory.GetCurrentDirectory(); }

            var downloadLocation = Path.Combine(directoryPath, Path.GetFileName(downloadUrl));
            var data = Task.Run(() => GetFileBytesAsync(downloadUrl)).Result;
            File.WriteAllBytes(downloadLocation, data);
        }

        /// <summary>
        /// Downloads the FFmpeg binary depending on the OS
        /// </summary>
        /// <param name="directoryPath">The optional directory of where it should be saved to</param>
        /// <exception cref="Exception"></exception>
        public static void DownloadFFmpeg(string directoryPath = "")
        {
            if (string.IsNullOrEmpty(directoryPath)) { directoryPath = Directory.GetCurrentDirectory(); }
            const string FFMPEG_API_URL = "https://ffbinaries.com/api/v1/version/latest";

            string jsonData = Task.Run(async () => await new HttpClient().GetStringAsync(FFMPEG_API_URL)).Result;

            JsonObject? jsonObj = JsonSerializer.Deserialize<JsonObject>(jsonData);

            if (jsonObj != null)
            {
                var ffmpegURL = OSHelper.GetOSVersion() switch
                {
                    OSVersion.Windows => JsonPeeker(jsonObj, new string[] { "bin", "windows-64", "ffmpeg" }),
                    OSVersion.OSX => JsonPeeker(jsonObj, new string[] { "bin", "osx-64", "ffmpeg" }),
                    OSVersion.Linux => JsonPeeker(jsonObj, new string[] { "bin", "linux-64", "ffmpeg" }),
                    _ => throw new Exception("Your OS isn't supported")
                };
                var dataBytes = Task.Run(async () => await GetFileBytesAsync(ffmpegURL)).Result;
                using (var stream = new MemoryStream(dataBytes))
                {
                    using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
                    {
                        if (archive.Entries.Count > 0)
                        {
                            archive.Entries[0].ExtractToFile(Path.Combine(directoryPath, archive.Entries[0].FullName), true);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Downloads a file from the specified URI
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Returns a byte array of the file that was downloaded</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static async Task<byte[]> GetFileBytesAsync(string uri)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out _))
                throw new InvalidOperationException("URI is invalid.");

            var httpClient = new HttpClient();
            byte[] fileBytes = await httpClient.GetByteArrayAsync(uri);
            return fileBytes;
        }

        /// <summary>
        /// Checks sections of a <see cref="JsonNode"/>, stopping if it hits a "null"
        /// </summary>
        /// <param name="json">Json object you want to look through</param>
        /// <param name="keys">List of properties you want to check through</param>
        /// <returns>Returns the object if it could find the value, or an empty string if a result turned up null</returns>
        private static string JsonPeeker(JsonObject json, string[] keys)
        {
            JsonNode? obj = null;
            for (int i = 0; i < keys.Length; i++)
            {
                if (obj == null)
                {
                    obj = json[keys[i]] ?? null;
                    if (obj == null) { break; }
                }
                else
                {
                    obj = obj[keys[i]] ?? null;
                    if (obj == null) { break; }
                }
            }
            var result = obj != null ? obj.ToString() : string.Empty;
            return result;
        }
    }
}
