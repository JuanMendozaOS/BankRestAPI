using BankRestAPI.Data;
using BankRestAPI.Models;
using BankRestAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankRestAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]s")]
    public class CustomerController : Controller
    {
        private readonly BankDbContext _dbContext;
        private readonly CustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;


        public CustomerController(BankDbContext dbContext, CustomerService customerService, ILogger<CustomerController> logger)
        {
            _dbContext = dbContext;
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        { 
            return Ok(await _customerService.GetAll());
        }

        [HttpGet("{documentNumber}")]
        public async Task<IActionResult> GetCustomer(string documentNumber)
        {
            if (string.IsNullOrEmpty(documentNumber)) { return BadRequest(); }
            var customer = await _customerService.GetById(documentNumber);
            if (customer != null)
            {
                return Ok(customer);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {

            if (ContainsNullOrEmpty(customer))
            {
                return BadRequest();
            }
            if (CustomerExists(customer))
            {
                return BadRequest("Customer already exists");
            }

            try
            {
                return StatusCode(201, await _customerService.Create(customer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut]
        [Route("{documentNumber}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] string documentNumber, [FromBody] Customer customer)
        {
            _logger.LogDebug("Dentro de update");
            var entity = await _customerService.GetById(documentNumber);

            if (HasNullValue(documentNumber, entity))
            {
                return BadRequest();
            }

            return Ok(Update(entity));
        }

        private async Task<Customer> Update(Customer entity)
        {
            if (!string.IsNullOrEmpty(entity.FullName))
            {
                entity.FullName = entity.FullName;
            }

            return await _customerService.Update(entity);
        }

        private bool HasNullValue(string documentNumber, Customer customer)
        {
            if (customer == null)
            {
                _logger.LogError("Customer is null");
                return true;
            }
            if (documentNumber == null)
            {
                _logger.LogError("DocumentNumber is null");
                return true;
            }
            return false;
        }

        private bool ContainsNullOrEmpty(Customer customer)
        {
            if (customer == null)
            {
                _logger.LogInformation("Customer object is null or empty");
                return true;
            }
            if (string.IsNullOrEmpty(customer.DocumentNumber))
            {
                _logger.LogInformation("DocumentNumber is null or empty");
                return true;
            }
            if (string.IsNullOrEmpty(customer.DocumentType))
            {
                _logger.LogInformation("DocumentType is null or empty");
                return true;
            }
            if (string.IsNullOrEmpty(customer.FullName))
            {
                _logger.LogInformation("FullName is null or empty");
                return true;
            }
            return false;
        }

        private bool CustomerExists(Customer customer)
        {
            if (_dbContext.Customer
                .Any(c => c.DocumentNumber == customer.DocumentNumber
                    && c.DocumentType == customer.DocumentNumber))
            {
                return true;
            }
            return false;
        }
    }

}

