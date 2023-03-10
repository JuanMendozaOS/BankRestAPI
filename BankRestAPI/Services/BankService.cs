using BankRestAPI.Data;
using BankRestAPI.Models.Bank;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankRestAPI.Services
{
    public class BankService : IEntityService<Bank>
    {

        private readonly BankDbContext _dbContext;

        public BankService(BankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Bank> Create(Bank entity)
        {
            await _dbContext.Banks.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Guid id)
        {
            var bank = await _dbContext.Banks.FindAsync(id);
            _dbContext.Banks.Remove(bank);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Bank>> GetAll()
        {
            return await _dbContext.Banks.ToListAsync();
        }

        public async Task<Bank> GetById(Guid id)
        {
            var bank = await _dbContext.Banks.FindAsync(id);
            return bank ?? null;
        }

        public async Task<Bank> Update(Bank entity)
        {
            _dbContext.Banks.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
