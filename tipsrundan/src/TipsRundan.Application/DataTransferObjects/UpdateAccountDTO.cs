using System.ComponentModel.DataAnnotations; // Importera detta namespace för att använda attributen nedan.

namespace TipsRundan.Application.DataTransferObjects
{
    public class UpdateAccountDTO
    {
        // Här identifierar vi vilket konto som ska uppdateras
        [Required]
        public Guid AccountId { get; set; }
        //Namn på användaren
        public string name { get; set; }
        //användarnamn här och samma som när man skapar måste det vara 3-20 tecken.
        [Required ( ErrorMessage = " Uername is a must if you want to update your account, ok? ")]
        [StringLength(20 , MinimumLength = 3, ErrorMessage = " 3 to 20 characters, that is the rule, ok?")]
        public string username { get; set; }
        // Uppdatera lösenordet, minst 8 tecken.
        [StringLength(20, MinimumLength = 8, ErrorMessage = " At least 8 characters, that is the rule, ok?")]
        public string password { get; set; }
        // Uppdatera mailadressen
        [EmailAddress(ErrorMessage = " What is this? A fake email?")]
        public string email { get; set; }
    }
}