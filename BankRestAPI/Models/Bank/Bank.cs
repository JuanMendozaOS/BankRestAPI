namespace BankRestAPI.Models.Bank
{
    public class Bank
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Adress { get; set; } = string.Empty;
    }
}
