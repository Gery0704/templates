using EBank.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBank.Admin.Model
{
    public interface IEBankModel
    {

        IReadOnlyList<BankAccountsDTO> BankAccounts { get; }
        IReadOnlyList<TransactionDTO> Transactions { get; }
        Boolean IsUserLoggedIn { get; }

        event EventHandler<BankAccountsEventArgs> BankAccountsChanged;

        void AddIn(TransactionDTO transaction);
        void TakeOut(TransactionDTO transaction);
        void Tran(TransactionDTO transaction);
        void LockBankAccount(BankAccountsDTO bankAccounts);

        Task LoadAsync();

        Task SaveAsync();

        Task<Boolean> LoginAsync(String userName, String userPassword);

        Task<Boolean> LogoutAsync();
    }
}
