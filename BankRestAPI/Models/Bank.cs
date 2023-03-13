using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankRestAPI.Models
{
    public class Bank { 
    
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Adress { get; set; } = default!;

        // Navigation properties
        [JsonIgnore]
        public List<Account>? Accounts { get; set; }
    }
}
