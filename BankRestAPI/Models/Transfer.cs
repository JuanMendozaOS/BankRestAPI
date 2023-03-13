namespace BankRestAPI.Models
{
    public class Transfer
    {
        public Guid Id { get; set; }
        // Datos de origen
        public Guid FromBankId;
    }
}
