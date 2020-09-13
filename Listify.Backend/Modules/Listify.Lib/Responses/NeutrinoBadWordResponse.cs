using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Responses
{
    public class NeutrinoBadWordResponse
    {
        public string Censoredcontent { get; set; }
        public bool Isbad { get; set; }
        public string[] Badwordslist { get; set; }
        public int Badwordstotal { get; set; }
    }
}
