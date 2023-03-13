using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankRestAPI.Models
{
    public class Account
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public long Number { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        //// Foreign Key
        //public string CustomerDocumentNumber { get; set; }
        //// Navigation property to Customer
        //public Customer? Customer { get; set; }

        // Navigation properties
        public Guid BankId { get; set; }
        [JsonIgnore]
        public Bank? Bank { get; set; }

    }
}
