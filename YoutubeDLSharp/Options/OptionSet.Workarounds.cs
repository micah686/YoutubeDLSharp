﻿// <auto-generated>
// This code was partially generated by a tool.
// </auto-generated>

namespace YoutubeDLSharp.Options
{
    public partial class OptionSet
    {
        private Option<string> encoding = new Option<string>("--encoding");
        //legacy server connect
        private Option<bool> noCheckCertificate = new Option<bool>("--no-check-certificate");
        private Option<bool> preferInsecure = new Option<bool>("--prefer-insecure");
        //private Option<string> userAgent = new Option<string>("--user-agent");
        //private Option<string> referer = new Option<string>("--referer");
        private Option<string> addHeader = new Option<string>("--add-header");
        private Option<bool> bidiWorkaround = new Option<bool>("--bidi-workaround");
        //sleep requests
        private Option<int?> sleepInterval = new Option<int?>("--sleep-interval");
        private Option<int?> maxSleepInterval = new Option<int?>("--max-sleep-interval");
        //sleep subtitles


        /// <summary>
        /// Force the specified encoding
        /// (experimental)
        /// </summary>
        public string Encoding { get => encoding.Value; set => encoding.Value = value; }
        /// <summary>
        /// Suppress HTTPS certificate validation
        /// </summary>
        public bool NoCheckCertificate { get => noCheckCertificate.Value; set => noCheckCertificate.Value = value; }
        /// <summary>
        /// Use an unencrypted connection to
        /// retrieve information about the video.
        /// (Currently supported only for YouTube)
        /// </summary>
        public bool PreferInsecure { get => preferInsecure.Value; set => preferInsecure.Value = value; }
        /// <summary>
        /// Specify a custom user agent
        /// </summary>
        public string UserAgent { get => userAgent.Value; set => userAgent.Value = value; }
        /// <summary>
        /// Specify a custom referer, use if the
        /// video access is restricted to one
        /// domain
        /// </summary>
        public string Referer { get => referer.Value; set => referer.Value = value; }
        /// <summary>
        /// Specify a custom HTTP header and its
        /// value, separated by a colon &#x27;:&#x27;. You
        /// can use this option multiple times
        /// </summary>
        public string AddHeader { get => addHeader.Value; set => addHeader.Value = value; }
        /// <summary>
        /// Work around terminals that lack
        /// bidirectional text support. Requires
        /// bidiv or fribidi executable in PATH
        /// </summary>
        public bool BidiWorkaround { get => bidiWorkaround.Value; set => bidiWorkaround.Value = value; }
        /// <summary>
        /// Number of seconds to sleep before each
        /// download when used alone or a lower
        /// bound of a range for randomized sleep
        /// before each download (minimum possible
        /// number of seconds to sleep) when used
        /// along with --max-sleep-interval.
        /// </summary>
        public int? SleepInterval { get => sleepInterval.Value; set => sleepInterval.Value = value; }
        /// <summary>
        /// Upper bound of a range for randomized
        /// sleep before each download (maximum
        /// possible number of seconds to sleep).
        /// Must only be used along with --min-
        /// sleep-interval.
        /// </summary>
        public int? MaxSleepInterval { get => maxSleepInterval.Value; set => maxSleepInterval.Value = value; }
    }
}
