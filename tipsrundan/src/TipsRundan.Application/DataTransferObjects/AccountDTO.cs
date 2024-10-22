namespace TipsRundan.Application.DataTransferObjects
{
    public class AccountDTO
    {
        public Guid AccountId { get; set; } // Unika konto-ID.
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
    }
}