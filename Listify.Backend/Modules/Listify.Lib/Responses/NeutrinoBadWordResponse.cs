using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Responses
{
    public class NeutrinoBadWordResponse
    {
        [JsonProperty(PropertyName = "censored-content")]
        public string Censoredcontent { get; set; }

        [JsonProperty(PropertyName = "is-bad")]
        public bool Isbad { get; set; }

        [JsonProperty(PropertyName = "bad-words-list")]
        public string[] Badwordslist { get; set; }

        [JsonProperty(PropertyName = "bad-words-total")]
        public int Badwordstotal { get; set; }
    }
}
