using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace YtDlpSharpLib.Metadata
{
    public class SubtitleInfo
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("ext")]
        public string Ext { get; set; }               

        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        //Unused Fields (These are fields that were excluded, but documented for future use:
        //http_headers
    }
}
