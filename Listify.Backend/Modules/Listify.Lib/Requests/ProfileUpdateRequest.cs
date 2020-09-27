using Listify.Domain.Lib.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Listify.Lib.Requests
{
    public class ProfileUpdateRequest : BaseUpdateRequest
    {
        public string ProfileTitle { get; set; }
        public string ProfileDescription { get; set; }
        public string AvatarUrl { get; set; }
    }
}
