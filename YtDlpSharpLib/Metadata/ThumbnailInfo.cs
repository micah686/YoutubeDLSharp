using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace YtDlpSharpLib.Metadata
{
    public class ThumbnailInfo
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("preference")]
        public int? Preference { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [JsonPropertyName("resolution")]
        public string Resolution { get; set; }

        [JsonPropertyName("filesize")]
        public int? Filesize { get; set; }

        //Unused Fields (These are fields that were excluded, but documented for future use:
        //http_headers

    }
}
