using BankRestAPI.Data;
using BankRestAPI.Models;
using BankRestAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankRestAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]s")]
    public class BankController : Controller
    {
        private readonly ILogger<BankController> _logger;
        private readonly IEntityService<Bank> _bankService;
        private readonly BankDbContext _dbContext;

        public BankController(ILogger<BankController> logger,
            IEntityService<Bank> bankService, BankDbContext dbContext)
        {
            _logger = logger;
            _bankService = bankService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetBanks()
        {
            return Ok(await _bankService.GetAll());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBank(Guid id)
        {
            if (id == Guid.Empty) { return BadRequest(); }
            var bank = await _bankService.GetById(id);
            if (bank != null)
            {
                return Ok(bank);
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> AddBank(Bank bank)
        {
            if (ContainsNullOrEmpty(bank)) { return BadRequest(); }
            if (BankExists(bank)){ return BadRequest(); }
            try
            {
                return StatusCode(201, await _bankService.Create(bank));

            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private bool BankExists(Bank bank)
        {
            if(_dbContext.Bank.Any(b => b.Name == bank.Name))
            {
                return true;
            }
            return false;
        }

       

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateBank(Guid id, Bank bank)
        {
            // TODO: Añadir validaciones
            if (id == Guid.Empty) { return BadRequest("Id cannot be empty"); }

            var entity = await _bankService.GetById(id);

            if (entity == null) { return NotFound($"Bank with id {id} not found"); }
            if (bank == null) { return BadRequest("Body cannot be empty"); }

            if (!string.IsNullOrEmpty(bank.Name))
            {
                entity.Name = bank.Name;
            }
            if (!string.IsNullOrEmpty(bank.Address))
            {
                entity.Address = bank.Address;
            }

            return Ok(await _bankService.Update(entity));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteBank(Guid id)
        {
            var bank = await _bankService.GetById(id);
            if (bank == null) { return NotFound($"Bank with id {id} not found"); }
            await _bankService.Delete(id);
            return Ok(await _bankService.GetAll());
        }

        private bool ContainsNullOrEmpty(Bank bank)
        {
            if (bank == null)
            {
                _logger.LogError("Bank object is null");
                return true;
            }
            if (string.IsNullOrEmpty(bank.Name))
            {
                _logger.LogError("BankName is null or empty");
                return true;
            }
            if (string.IsNullOrEmpty(bank.Address))
            {
                _logger.LogError("BankAddress is null or empty");
                return true;
            }

            return false;
        }
    }
}
