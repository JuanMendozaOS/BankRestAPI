using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankRestAPI.Models
{
    public class Account
    {
        public Guid Id { get; set; } = default!;
        public long Number { get; set; } = default!;
        public string Currency { get; set; } = string.Empty;
        public decimal Balance { get; set; } = default!;

        //Navigation property to Customer
        public Customer? Customer { get; set; } = default!;

        // Navigation properties to Bank
        public Bank? Bank { get; set; } = default!;

    }
}
