using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EBank.Data;
using EBank.Szervic.Models;

namespace EBank.Szervic.Controllers
{
    [Route("api/[controller]")]
    public class TransactionsController : Controller
    {
        private readonly EBankContext _context;

        public TransactionsController(EBankContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult PostTransaction([FromBody] TransactionDTO transactionDTO)
        {
            try
            {
                var addedTran = _context.Transactions.Add(new Transactions
                {
                    AccountID = transactionDTO.AccountID,
                    AccountNumberFrom = transactionDTO.AccountNumberFrom,
                    AccountNumberTo = transactionDTO.AccountNumberTo,
                    Created = transactionDTO.Created,
                    ReceiverName = transactionDTO.ReceiverName,
                    TransactionType = transactionDTO.TransactionType,
                    TransactionValue = transactionDTO.TransactionValue
                });

                _context.SaveChanges();


                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}