using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace YtDlpSharpLib.Metadata
{
    public class VideoInfo
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("formats")]
        public FormatInfo[] Formats { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("ext")]
        public string Extension { get; set; }
        [JsonPropertyName("format")]
        public string Format { get; set; }
        [JsonPropertyName("player_url")]
        public string PlayerUrl { get; set; }
        // Additional optional fields:
        [JsonPropertyName("direct")]
        public bool Direct { get; set; }
        [JsonPropertyName("alt_title")]
        public string AltTitle { get; set; }
        [JsonPropertyName("display_id")]
        public string DisplayID { get; set; }
        [JsonPropertyName("thumbnails")]
        public ThumbnailInfo[] Thumbnails { get; set; }
        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("uploader")]
        public string Uploader { get; set; }
        [JsonPropertyName("license")]
        public string License { get; set; }
        [JsonPropertyName("creator")]
        public string Creator { get; set; }
        [JsonPropertyName("timestamp")]
        public long? Timestamp { get; set; }
        [JsonPropertyName("upload_date")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? UploadDate { get; set; } // date in UTC (YYYYMMDD).
        [JsonPropertyName("release_timestamp")]
        public long? ReleaseTimestamp { get; set; } // date as unix timestamp
        [JsonPropertyName("release_date")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? ReleaseDate { get; set; } // date in UTC (YYYYMMDD).
        [JsonPropertyName("modified_timestamp")]
        public long? ModifiedTimestamp { get; set; } // date as unix timestamp
        [JsonPropertyName("modified_date")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? ModifiedDate { get; set; } // date in UTC (YYYYMMDD).
        [JsonPropertyName("uploader_id")]
        public string UploaderID { get; set; }
        [JsonPropertyName("uploader_url")]
        public string UploaderUrl { get; set; }
        [JsonPropertyName("channel")]
        public string Channel { get; set; }
        [JsonPropertyName("channel_id")]
        public string ChannelID { get; set; }
        [JsonPropertyName("channel_url")]
        public string ChannelUrl { get; set; }
        [JsonPropertyName("channel_follower_count")]
        public long? ChannelFollowerCount { get; set; }
        [JsonPropertyName("location")]
        public string Location { get; set; }
        [JsonPropertyName("subtitles")]
        public Dictionary<string, SubtitleInfo[]> Subtitles { get; set; }
        [JsonPropertyName("automatic_captions")]
        public Dictionary<string, SubtitleInfo[]> AutomaticCaptions { get; set; }
        [JsonPropertyName("duration")]
        public float? Duration { get; set; }
        [JsonPropertyName("view_count")]
        public long? ViewCount { get; set; }

        //concurrent_view

        [JsonPropertyName("like_count")]
        public long? LikeCount { get; set; }
        [JsonPropertyName("dislike_count")]
        public long? DislikeCount { get; set; }
        [JsonPropertyName("repost_count")]
        public long? RepostCount { get; set; }
        [JsonPropertyName("average_rating")]
        public double? AverageRating { get; set; }
        [JsonPropertyName("comment_count")]
        public long? CommentCount { get; set; }
        [JsonPropertyName("comments")]
        public CommentInfo[] Comments { get; set; }
        [JsonPropertyName("age_limit")]
        public int? AgeLimit { get; set; }
        [JsonPropertyName("webpage_url")]
        public string WebpageUrl { get; set; }
        [JsonPropertyName("categories")]
        public string[] Categories { get; set; }
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; }
        [JsonPropertyName("cast")]
        public string Cast { get; set; }//===============Validate if string or string array
        [JsonPropertyName("is_live")]
        public bool? IsLive { get; set; }
        [JsonPropertyName("was_live")]
        public bool? WasLive { get; set; }
        [JsonPropertyName("live_status")]
        public LiveStatus LiveStatus { get; set; }
        [JsonPropertyName("start_time")]
        public float? StartTime { get; set; }
        [JsonPropertyName("end_time")]
        public float? EndTime { get; set; }
        [JsonPropertyName("playable_in_embed")]
        public string PlayableInEmbed { get; set; }
        [JsonPropertyName("availability")]
        public Availability Availability { get; set; }
        [JsonPropertyName("chapters")]
        public ChapterInfo[] Chapters { get; set; }
        [JsonPropertyName("chapter")]
        public string Chapter { get; set; }
        [JsonPropertyName("chapter_number")]
        public int ChapterNumber { get; set; }
        [JsonPropertyName("chapter_id")]
        public string ChapterId { get; set; }
        [JsonPropertyName("series")]
        public string Series { get; set; }
        [JsonPropertyName("series_id")]
        public string SeriesId { get; set; }
        [JsonPropertyName("season")]
        public string Season { get;  set; }
        [JsonPropertyName("season_number")]
        public int SeasonNumber { get; set; }
        [JsonPropertyName("season_id")]
        public string SeasonId { get; set; }
        [JsonPropertyName("episode")]
        public string Episode { get; set; }
        [JsonPropertyName("episode_number")]
        public int EpisodeNumber { get; set; }
        [JsonPropertyName("episode_id")]
        public string EpisodeId { get; set; }
        [JsonPropertyName("track")]
        public string Track { get; set; }
        [JsonPropertyName("track_number")]
        public int TrackNumber { get; set; }
        [JsonPropertyName("track_id")]
        public string TrackId { get; set; }
        [JsonPropertyName("artist")]
        public string Artist { get; set; }//===============Validate if string or string array
        [JsonPropertyName("genre")]
        public string Genre { get; set; }//===============Validate if string or string array
        [JsonPropertyName("album")]
        public string Album { get; set; }
        [JsonPropertyName("album_type")]
        public string AlbumType { get; set; }
        [JsonPropertyName("album_artist")]
        public string AlbumArtist { get; set; }//===============Validate if string or string array
        [JsonPropertyName("disc_number")]
        public int DiscNumber { get; set; }
        [JsonPropertyName("release_year")]
        public string ReleaseYear { get; set; }
        [JsonPropertyName("composer")]
        public string Composter { get; set; }





        [JsonPropertyName("section_start")]
        public long? SectionStart { get; set; }
        [JsonPropertyName("section_end")]
        public long? SectionEnd { get; set; }
        [JsonPropertyName("rows")]
        public long? StoryboardFragmentRows { get; set; }
        [JsonPropertyName("columns")]
        public long? StoryboardFragmentColumns { get; set; }














        //======================================================
        [JsonPropertyName("_type")]
        public MetadataType ResultType { get; set; }
       
        [JsonPropertyName("extractor")]
        public string Extractor { get; set; }
        [JsonPropertyName("extractor_key")]
        public string ExtractorKey { get; set; }

        // If data refers to a playlist:
        [JsonPropertyName("entries")]
        public VideoData[] Entries { get; set; }


        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }






    }    

}
