using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Responses
{
    public class SpotifyPlaylistTracksResponse
    {
        public string href { get; set; }
        public Item[] items { get; set; }
        public int limit { get; set; }
        public object next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }

        public class Item
        {
            public DateTime added_at { get; set; }
            public Added_By added_by { get; set; }
            public bool is_local { get; set; }
            public Track track { get; set; }
        }

        public class Added_By
        {
            public External_Urls external_urls { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class External_Urls
        {
            public string spotify { get; set; }
        }

        public class Track
        {
            public Album album { get; set; }
            public Artist1[] artists { get; set; }
            public string[] available_markets { get; set; }
            public int disc_number { get; set; }
            public int duration_ms { get; set; }
            public bool _explicit { get; set; }
            public External_Ids external_ids { get; set; }
            public External_Urls3 external_urls { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public int popularity { get; set; }
            public string preview_url { get; set; }
            public int track_number { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class Album
        {
            public string album_type { get; set; }
            public Artist[] artists { get; set; }
            public string[] available_markets { get; set; }
            public External_Urls1 external_urls { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public Image[] images { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class External_Urls1
        {
            public string spotify { get; set; }
        }

        public class Artist
        {
            public External_Urls2 external_urls { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class External_Urls2
        {
            public string spotify { get; set; }
        }

        public class Image
        {
            public int height { get; set; }
            public string url { get; set; }
            public int width { get; set; }
        }

        public class External_Ids
        {
            public string isrc { get; set; }
        }

        public class External_Urls3
        {
            public string spotify { get; set; }
        }

        public class Artist1
        {
            public External_Urls4 external_urls { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class External_Urls4
        {
            public string spotify { get; set; }
        }

    }
}
