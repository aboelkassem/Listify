using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Responses
{
    public class YoutubeSearchResponse
    {
        public string Kind { get; set; }
        public string Etag { get; set; }
        public Item[] Items { get; set; }
    }

    public class Item
    {
        public string Kind { get; set; }
        public string Etag { get; set; }
        public string Id { get; set; }
        public ContentDetails ContentDetails { get; set; }
    }

    public class ContentDetails
    {
        public string Duration { get; set; }
        public string Dimension { get; set; }
        public string Definition { get; set; }
        public bool Caption { get; set; }
        public bool LicensedContent { get; set; }
    }
}
