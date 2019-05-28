using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBank.Data;
using EBank.Szervic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBank.Szervic.Controllers
{
    [Route("api/[controller]")]
    public class BankAccountsController : Controller
    {
        private readonly EBankContext _context;

        public BankAccountsController(EBankContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAccounts()
        {

            try
            {
                return Ok(_context.BankAccounts.ToList().Select(ba => new BankAccountsDTO
                {
                    ID = ba.ID,
                    ClientName = _context.Users.FirstOrDefault(c => c.Id == ba.ClientID).FullName,
                    AccountNumber = ba.AccountNumber,
                    Balance = ba.Balance,
                    Created = ba.Created,
                    isLocked = ba.isLocked
                }));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        public IActionResult GetAccountsMock()
        {

            try
            {
                return Ok(_context.BankAccounts.ToList().Select(ba => new BankAccountsDTO
                {
                    ID = ba.ID,
                    AccountNumber = ba.AccountNumber,
                    Balance = ba.Balance,
                    Created = ba.Created,
                    isLocked = ba.isLocked
                }));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult PutBankAccounts([FromBody] BankAccountsDTO bankAccountsDTO)
        {
            try
            {
                BankAccounts bankAccounts = _context.BankAccounts.FirstOrDefault(b => b.ID == bankAccountsDTO.ID);

                if (bankAccounts == null) // ha nincs ilyen azonosító, akkor hibajelzést küldünk
                    return NotFound();

                bankAccounts.Balance = bankAccountsDTO.Balance;
                bankAccounts.isLocked = bankAccountsDTO.isLocked;

                _context.SaveChanges(); // elmentjük a módosított épületet

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        private bool BankAccountsExists(int id)
        {
            return _context.BankAccounts.Any(e => e.ID == id);
        }
    }
}