using EBank.Data;
using EBank.Szervic.Controllers;
using EBank.Szervic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EBank.Service.Test
{
    public class EBankTest : IDisposable
    {
        private readonly EBankContext _context;
        private readonly List<BankAccountsDTO> _bankAccountsDTOs;
        private readonly List<TransactionDTO> _transactionDTOs;

        public EBankTest()
        {
            var options = new DbContextOptionsBuilder<EBankContext>()
                .UseInMemoryDatabase("EbankTest")
                .Options;

            _context = new EBankContext(options);
            _context.Database.EnsureCreated();

            // adatok inicializációja
            var ClientsData = new List<Clients>
            {
                new Clients { Id = 1, FullName = "Teszt1 Elek"},
                new Clients { Id = 2, FullName = "Teszt2 Elek"}
            };
            _context.Users.AddRange(ClientsData);

            var BankAccountsData = new List<BankAccounts>
        {
            new BankAccounts { ID = 1, AccountNumber = "12345678-12345678-12345601", Balance = 5000, isLocked = false, ClientID = 1, Created = DateTime.Parse("2019-05-19 18:43:05.2675229") },
            new BankAccounts { ID = 2, AccountNumber = "12345678-12345678-12345602", Balance = 5000, isLocked = false, ClientID = 2, Created = DateTime.Parse("2019-05-19 18:43:05.2675229") }
        };
            _context.BankAccounts.AddRange(BankAccountsData);
            _context.SaveChanges();

            _bankAccountsDTOs = BankAccountsData.Select(bankAccount => new BankAccountsDTO
            {
                ID = bankAccount.ID,
                AccountNumber = bankAccount.AccountNumber,
                Balance = bankAccount.Balance,
                isLocked = bankAccount.isLocked,
                ClientName = ClientsData.FirstOrDefault(c => c.Id == bankAccount.ClientID).FullName,
                Created = bankAccount.Created
            }).ToList();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void GetAccountsTest()
        {
            var controller = new BankAccountsController(_context);
            var result = controller.GetAccounts();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BankAccountsDTO>>(objectResult.Value);
            Assert.Equal(_bankAccountsDTOs, model);
        }
    }
}
