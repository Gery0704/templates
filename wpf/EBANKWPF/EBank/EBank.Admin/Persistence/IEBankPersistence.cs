using EBank.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBank.Admin.Persistence
{
    public interface IEBankPersistence
    {
        Task<IEnumerable<BankAccountsDTO>> ReadAccountsAsync();

        Task<Boolean> UpdateAccountAsync(BankAccountsDTO bankAccounts);
        Task<Boolean> CreateTranAsync(TransactionDTO transaction);
        Task<Boolean> LoginAsync(String userName, String userPassword);

        Task<Boolean> LogoutAsync();
    }
}
