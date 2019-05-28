using EBank.Admin.Persistence;
using EBank.Data;
using EBank.Szervic.Controllers;
using EBank.Szervic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EBank.Service.Test
{
    public class EbankMockTest
    {
        private readonly List<BankAccountsDTO> _bankAccountsDTOs;
        
        private Mock<DbSet<BankAccounts>> _bankAccountsMock;
        private Mock<DbSet<Clients>> _clientsMock;
        private Mock<EBankContext> _entityMock;

        public EbankMockTest()
        {
            var ClientsData = new List<Clients>
        {
            new Clients { Id = 1, FullName = "Teszt1 Elek"},
            new Clients { Id = 2, FullName = "Teszt2 Elek"}
        };

            var BankAccountsData = new List<BankAccounts>
        {
            new BankAccounts { ID = 1, AccountNumber = "12345678-12345678-12345601", Balance = 5000, isLocked = false, ClientID = 1, Created = DateTime.Parse("2019-05-19 18:43:05.2675229") },
            new BankAccounts { ID = 2, AccountNumber = "12345678-12345678-12345602", Balance = 5000, isLocked = false, ClientID = 2, Created = DateTime.Parse("2019-05-19 18:43:05.2675229") }
        };

            _bankAccountsDTOs = BankAccountsData.Select(bankAccount => new BankAccountsDTO
            {
                ID = bankAccount.ID,
                AccountNumber = bankAccount.AccountNumber,
                Balance = bankAccount.Balance,
                isLocked = bankAccount.isLocked,
                Created = bankAccount.Created
            }).ToList();

            IQueryable<BankAccounts> queryableBankAccountsData = BankAccountsData.AsQueryable();
            _bankAccountsMock = new Mock<DbSet<BankAccounts>>();
            _bankAccountsMock.As<IQueryable<BankAccounts>>().Setup(mock => mock.ElementType).Returns(queryableBankAccountsData.ElementType);
            _bankAccountsMock.As<IQueryable<BankAccounts>>().Setup(mock => mock.Expression).Returns(queryableBankAccountsData.Expression);
            _bankAccountsMock.As<IQueryable<BankAccounts>>().Setup(mock => mock.Provider).Returns(queryableBankAccountsData.Provider);
            _bankAccountsMock.As<IQueryable<BankAccounts>>().Setup(mock => mock.GetEnumerator()).Returns(BankAccountsData.GetEnumerator());

            _entityMock = new Mock<EBankContext>();
            _entityMock.Setup<DbSet<BankAccounts>>(entity => entity.BankAccounts).Returns(_bankAccountsMock.Object);

        }

        [Fact]
        public void GetAccountsTest()
        {
            var controller = new BankAccountsController(_entityMock.Object);
            var result = controller.GetAccountsMock();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BankAccountsDTO>>(objectResult.Value);
            Assert.Equal(_bankAccountsDTOs, model);
        }
        
    }
}
