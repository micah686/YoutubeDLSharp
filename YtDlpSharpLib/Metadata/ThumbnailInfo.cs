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
        [JsonPropertyName("filesize")]
        public int? Filesize { get; set; }

        [JsonPropertyName("height")]
        public int? Height { get; set; }

        //http_headers

        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        [JsonPropertyName("preference")]
        public int? Preference { get; set; }

        [JsonPropertyName("resolution")]
        public string Resolution { get; set; }

        [JsonPropertyName("width")]
        public int? Width { get; set; }
                
        [JsonPropertyName("url")]
        public string Url { get; set; }

    }
}
