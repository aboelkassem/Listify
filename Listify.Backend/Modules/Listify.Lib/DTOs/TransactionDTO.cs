using Listify.Domain.Lib.DTOs;
using Listify.Domain.Lib.Enums;

namespace Listify.Lib.DTOs
{
    // This could be gaining currency for being in a room
    // or this could be using currency to request/ up-vote / down-vote songs.
    public class TransactionDTO : BaseDTO
    {
        public TransactionType TransactionType { get; set; }
        public int QuantityChange { get; set; }
    }
}
