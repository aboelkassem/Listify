using System.Threading.Tasks;

namespace Listify.Services
{
    public interface IListifyService
    {
        Task<string> CleanContent(string content);
        Task<bool> IsContentValid(string content);
    }
}