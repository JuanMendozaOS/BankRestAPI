using BankRestAPI.Models.Bank;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace BankRestAPI.Data
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Bank> Banks { get; set; }
    }
}
