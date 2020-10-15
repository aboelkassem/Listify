using Listify.Lib.Requests;
using Listify.Lib.Responses;
using System.Threading.Tasks;

namespace Listify.Services
{
    public interface IListifyService
    {
        Task<string> GetSpotifyAccessToken();
        Task<string> CleanContent(string content);
        Task<bool> IsContentValid(string content);
    }
}