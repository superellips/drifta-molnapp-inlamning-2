using TipsRundan.Application.DataTransferObjects;
using System.ComponentModel.DataAnnotations;

namespace TipsRundan.Application.DataTransferObjects
{
    public class DeleteAccountDTO
    {
        // Kontot raderas med hjälpa av id. Användaren ska då vara inloggad för att radera sitt konto.
        [Required(ErrorMessage = " An account id is required to delete an account.")]
        public Guid AccountId { get; set; }
    }
}