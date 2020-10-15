using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Responses
{
    public class SpotifyAccessTokenResponse
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_in { get; set; }
        public string Scope { get; set; }
    }
}
