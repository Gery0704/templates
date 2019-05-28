using EBank.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EBank.Admin.Persistence
{
    public class EBankPersistence : IEBankPersistence
    {
        private HttpClient _client;

        public EBankPersistence(String baseAddress)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseAddress);
        }

        public EBankPersistence(HttpClient client)
        {
            _client = client;
        }

        public async Task<Boolean> UpdateAccountAsync(BankAccountsDTO bankAccounts)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/bankaccounts/", bankAccounts);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> CreateTranAsync(TransactionDTO transaction)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/transactions/", transaction);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
        public async Task<IEnumerable<BankAccountsDTO>> ReadAccountsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/bankaccounts/");
            if (response.IsSuccessStatusCode) // amennyiben sikeres a művelet
            {
                IEnumerable<BankAccountsDTO> bankAccounts = await response.Content.ReadAsAsync<IEnumerable<BankAccountsDTO>>();


                return bankAccounts;
            }
            else return null;
        }
        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            var cc = _client;
            HttpResponseMessage response = await _client.GetAsync("api/account/login/" + userName + "/" + userPassword);
            var tmp = response;
            return response.IsSuccessStatusCode;
        }

        public async Task<Boolean> LogoutAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/account/logout");
            return !response.IsSuccessStatusCode;

        }
    }
}
