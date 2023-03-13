using BankRestAPI.Data;
using BankRestAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankRestAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]s")]
    public class AccountController : Controller
    {
        public BankDbContext _dbContext;

        public AccountController(BankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _dbContext.Account.ToListAsync();
        }

        

        [HttpPost]
        public async Task<IActionResult> AddAccount(Account account)
        {
            await _dbContext.Account.AddAsync(account);
            await _dbContext.SaveChangesAsync();
            return Ok(account);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccount(Guid id)
        {
            return Ok();
        }

        
    }
}
