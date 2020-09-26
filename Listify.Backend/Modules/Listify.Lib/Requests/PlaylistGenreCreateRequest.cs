using Listify.Domain.Lib.Requests;
using Listify.Lib.DTOs;
using System;

namespace Listify.Lib.Requests
{
    public class PlaylistGenreCreateRequest : BaseRequest
    {
        public GenreDTO Genre { get; set; }
    }
}
