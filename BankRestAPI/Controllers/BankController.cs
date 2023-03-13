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

        public BankController(ILogger<BankController> logger,
            IEntityService<Bank> bankService)
        {
            _logger = logger;
            _bankService = bankService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBanks()
        {
            _logger.LogWarning("Prueba logger");
            return Ok(await _bankService.GetAll());
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBank(Guid id)
        {
            return Ok(await _bankService.GetById(id));
        }


        [HttpPost]
        public async Task<IActionResult> AddBank(Bank bank)
        {
            return Ok(await _bankService.Create(bank));
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
            if (!string.IsNullOrEmpty(bank.Adress))
            {
                entity.Adress = bank.Adress;
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

    }
}
