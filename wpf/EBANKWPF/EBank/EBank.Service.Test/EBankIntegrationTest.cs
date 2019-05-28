using EBank.Admin.Persistence;
using EBank.Data;
using EBank.Szervic.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace EBank.Service.Test
{
    public class EBankIntegrationTest
    {

        public static IList<Clients> ClientsData = new List<Clients>
        {
            new Clients { Id = 1, FullName = "Teszt1 Elek"},
            new Clients { Id = 2, FullName = "Teszt2 Elek"}
        };
        public static IList<BankAccounts> BankAccountsData = new List<BankAccounts>
        {
            new BankAccounts { ID = 1, AccountNumber = "12345678-12345678-12345601", Balance = 5000, isLocked = false, ClientID = 1, Created = DateTime.Parse("2019-05-19 18:43:05.2675229") },
            new BankAccounts { ID = 2, AccountNumber = "12345678-12345678-12345602", Balance = 5000, isLocked = false, ClientID = 2, Created = DateTime.Parse("2019-05-19 18:43:05.2675229") }
        };
        
        private readonly List<BankAccountsDTO> _bankAccountsDTOs;
        private readonly List<TransactionDTO> _transactionDTOs;
        private readonly IEBankPersistence _persistence;

        public EBankIntegrationTest()
        {
            _bankAccountsDTOs = BankAccountsData.Select(bankAccount => new BankAccountsDTO
            {
                ID = bankAccount.ID,
                AccountNumber = bankAccount.AccountNumber,
                Balance = bankAccount.Balance,
                isLocked = bankAccount.isLocked,
                ClientName = ClientsData.FirstOrDefault(c => c.Id == bankAccount.ClientID).FullName,
                Created = bankAccount.Created
            }).ToList();
            

            var webHostBuilder = new WebHostBuilder() // szerver konfiguráció összeállítása
                .UseStartup<TestStartup>()
                .UseEnvironment("Development");

            var server = new TestServer(webHostBuilder); // szerver példányosítása

            var client = server.CreateClient(); // kliens példányosítása
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _persistence = new EBankPersistence(client);
        }

        [Fact]
        public void GetBankAccountsTest()
        {
            IEnumerable<BankAccountsDTO> result = _persistence.ReadAccountsAsync().Result;

            result.Equals(_bankAccountsDTOs);
            Assert.Equal(_bankAccountsDTOs, result);
        }

    }
}
