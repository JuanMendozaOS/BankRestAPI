using BankRestAPI.Data;
using BankRestAPI.Models;
using BankRestAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankRestAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]s")]
    public class TransferController : Controller
    {
        private readonly ILogger<TransferController> _logger;
        private readonly BankDbContext _bankDbContext;
        private readonly TransferService _transferService;
        private readonly BankService _bankService;
        private readonly AccountService _accountService;
        private readonly CustomerService _customerService;

        public TransferController(ILogger<TransferController> logger, BankDbContext bankDbContext, TransferService transferService, BankService bankService, CustomerService customerService, AccountService accountService)
        {
            _logger = logger;
            _bankDbContext = bankDbContext;
            _transferService = transferService;
            _bankService = bankService;
            _accountService = accountService;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransfers()
        {
            return Ok(await _transferService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Send(Transfer transfer)
        {
            try
            {
                if (ContainsNullOrEmpty(transfer))
                {
                    return BadRequest();
                }
                transfer.TransactionState = "EN PROCESO";

                var fromBank = await _bankService.GetByCode(transfer.FromBank.Code);
                var toBank = await _bankService.GetByCode(transfer.ToBank.Code);
                var fromAccount = await _accountService.GetById(transfer.FromAccount.Id);
                var toAccount = await _accountService.GetById(transfer.ToAccount.Id);
                var fromCustomer = await _customerService.GetById(transfer.FromCustomer.DocumentNumber);
                var toCustomer = await _customerService.GetById(transfer.FromCustomer.DocumentNumber);

                if (fromBank == null || toBank == null)
                {
                    _logger.LogError("Al menos uno de los bancos no existe");
                    return BadRequest();
                }

                if (fromBank == toBank)
                {
                    throw new Exception("Bancos NO deben ser Iguales");
                }

                if (fromAccount == null || toAccount == null)
                {
                    _logger.LogError("Al menos una de las cuentas no existe");
                    return BadRequest();
                }

                if (!validateTransferAmount(transfer, fromAccount))
                {
                    throw new Exception($"Insuficiencia de fondos: {fromAccount.Balance}");
                }

                if(!validateCustomers(fromCustomer, toCustomer))
                {
                    return NotFound("Al menos uno de los clientes no existe");
                }

                fromAccount.Balance = fromAccount.Balance - transfer.Amount;
                toAccount.Balance = toAccount.Balance + transfer.Amount;

                await _accountService.Update(fromAccount);
                await _accountService.Update(toAccount);

                transfer.FromBank = fromBank;
                transfer.ToBank = toBank;   
                transfer.FromAccount = fromAccount;
                transfer.ToAccount = toAccount;
                transfer.FromCustomer = fromCustomer;
                transfer.ToCustomer = toCustomer;
                transfer.OperationDate = DateTime.Now.ToUniversalTime();
                transfer.TransactionState = "FINALIZADO";

                await _transferService.Create(transfer);
                return StatusCode(201, transfer);

            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            
        }

        private bool validateCustomers(Customer? fromCustomer, Customer? toCustomer)
        {
            if(fromCustomer == null || toCustomer == null)
            {
                _logger.LogError("Al menos uno de los clientes no existe");
                return false;
            }
            return true;
        }

        private bool validateTransferAmount(Transfer transfer, Account fromAccount)
        {
            var amount = transfer.Amount;

            if (amount < 0) 
            {
                _logger.LogError($"Monto de transferencia menor a 0: ${amount}"); 
                throw new Exception($"Monto inválido: {amount}");
            }
            if(amount > fromAccount.Balance)
            {
                _logger.LogError($"Monto de transferencia es mayor al balance de cuenta: {transfer.Currency} {amount}");
                return false;
            }

            return true;
        }

        [HttpPut("TransactionState/{id:guid}")]
        public async Task<IActionResult> UpdateTransactionState(Guid id, string transactionState)
        {
            throw new NotImplementedException();
        }

        [HttpGet("TransactionState/{id:guid}")]
        public async Task<IActionResult> GetTransactionState(Guid id)
        {
            throw new NotImplementedException();
        }

        private bool ContainsNullOrEmpty(Transfer transfer)
        {
            if (transfer == null)
            {
                _logger.LogError("Transfer object is null");
                return true;
            }

            if (string.IsNullOrEmpty(transfer.Currency))
            {
                _logger.LogError("Currency es null o empty");
                return true;
            }

            // origin data 
            if (string.IsNullOrEmpty(transfer.FromBankName))
            {
                _logger.LogError("Origin Bank Name es null o empty");
                return true;
            }
            if (transfer.FromBank == null)
            {
                _logger.LogError("Origin Bank object es null");
                return true;
            }
            if (string.IsNullOrEmpty(transfer.FromBank.Code))
            {
                _logger.LogError("Origin Bank Code es null o empty");
                return true;
            }
            if (string.IsNullOrEmpty(transfer.FromCustomer.DocumentNumber))
            {
                _logger.LogError("Origin Document Number es null o empty");
                return true;
            }
            if (transfer.FromAccount == null)
            {
                _logger.LogError("Origin Account es null o empty");
                return true;
            }
            if (transfer.FromAccount.Id == Guid.Empty)
            {
                _logger.LogError("Origin Account Id es null o empty");
                return true;
            }

            // receiver data

            if (string.IsNullOrEmpty(transfer.ToBankName))
            {
                _logger.LogError("Receiver Bank Name es null or empty");
                return true;
            }
            if (transfer.ToBank == null)
            {
                _logger.LogError("Receiver Bank object es null");
                return true;
            }

            if (string.IsNullOrEmpty(transfer.ToBank.Code))
            {
                _logger.LogError("Receiver Bank Code es null or empty");
                return true;
            }
            if (string.IsNullOrEmpty(transfer.ToCustomer.DocumentNumber))
            {
                _logger.LogError("Receiver Document Number es null or empty");
                return true;
            }
            if (transfer.ToAccount == null)
            {
                _logger.LogError("Receiver Account es null or empty");

                return true;
            }
            if (transfer.ToAccount.Id == Guid.Empty)
            {
                _logger.LogError("Receiver Account Id es null or empty");
                return true;
            }

            return false;
        }

    }
}
