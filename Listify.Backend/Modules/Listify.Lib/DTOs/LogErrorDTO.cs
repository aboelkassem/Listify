using Listify.Domain.Lib.DTOs;

namespace Listify.Lib.DTOs
{
    public class LogErrorDTO : BaseDTO
    {
        public string Exception { get; set; }
        public string IPAddress { get; set; }
    }
}
