using System.ComponentModel.DataAnnotations;

namespace TipsRundan.Application.DataTransferObjects
{
    public class CreateAccountDTO
    {   
        public string name { get; set;}
        //Lägg till användarnamn och regler. Användarnamn måste finnas. MIn 3 tecken, max 20 tecken.
        [Required (ErrorMessage= " Woops, you forgot to enter a username you silly goose!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Well, that is not a very good username, is it? It has to be between 3 and 20 characters.")]
        public string username { get; set; }

        // lägg till ditt lösenord som är obligatoriskt och minst 8 tecken långt.
        [Required (ErrorMessage = " Do you want to get hacked? You need a password!")]
        [StringLength( 50, MinimumLength = 8, ErrorMessage = " Good one, to bad it has to be at least 8 characters long.")]
        public string password { get; set; }
        // Skriv ner din mailadress
        [Required ( ErrorMessage = " Without an email, how will we be able to contact you? ")]
        [EmailAddress( ErrorMessage = " You can't fool me, that is not a valid email address.")] // MOdelstate.isvalid i Controller.
        public string email { get; set; }


    }
}