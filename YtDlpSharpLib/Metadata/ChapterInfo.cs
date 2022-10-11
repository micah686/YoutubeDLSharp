using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace YtDlpSharpLib.Metadata
{
    public class ChapterInfo
    {
        [JsonPropertyName("start_time")]
        public long StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public long EndTime { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
