using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Responses
{
    public class YoutubePlaylistSearchResponse
    {
        public string Kind { get; set; }
        public string Etag { get; set; }
        public Item[] Items { get; set; }

        public class Item
        {
            public string Kind { get; set; }
            public string Etag { get; set; }
            public string Id { get; set; }
            public Snippet Snippet { get; set; }
        }
        public class Snippet
        {
            public string PublishedAt { get; set; }
            public string ChannelId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public Thumbnails Thumbnails { get; set; }
            public string ChannelTitle { get; set; }
            public string PlaylistId { get; set; }
            public int Position { get; set; }
            public Resource ResourceId { get; set; }
        }

        public class Resource
        {
            public string Kind { get; set; }
            public string VideoId { get; set; }
        }
        public class Thumbnails
        {
            public ThumbnailDetails Default { get; set; }
            public ThumbnailDetails Medium { get; set; }
            public ThumbnailDetails High { get; set; }
            public ThumbnailDetails Standard { get; set; }
            public ThumbnailDetails Maxres { get; set; }
        }
        public class ThumbnailDetails
        {
            public string Url { get; set; }
            public long? Width { get; set; }
            public long? Height { get; set; }
        }
    }
}
