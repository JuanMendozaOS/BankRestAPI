using BankRestAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;

namespace BankRestAPI.Data
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Bank> Bank { get; set; }
        public DbSet<Customer> Customer { get; set; }  
        public DbSet<Account> Account { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Account>()
        //        .HasOne(a => a.Bank) // An account has one bank
        //        .WithMany()
        //        .HasForeignKey(a => a.BankId);
        //    modelBuilder.Entity<Account>()
        //        .HasOne(a => a.Bank) // An account has one bank
        //        .WithMany()
        //        .HasForeignKey(a => a.BankId);


        //    base.OnModelCreating(modelBuilder);

        //}

    }
}
