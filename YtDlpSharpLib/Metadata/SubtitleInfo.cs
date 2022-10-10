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
        [JsonPropertyName("ext")]
        public string Ext { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

        //http_headers

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
