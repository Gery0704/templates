using EBank.Admin.Persistence;
using EBank.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBank.Admin.Model
{
    public class EBankModel : IEBankModel
    {
        private enum DataFlag
        {
            Create,
            Update,
            Delete
        }

        private IEBankPersistence _persistence;
        private List<BankAccountsDTO> _bankAccounts;
        private List<TransactionDTO> _transactions = new List<TransactionDTO>();
        private Dictionary<BankAccountsDTO, DataFlag> _bankAccountsFlags;
        private Dictionary<TransactionDTO, DataFlag> _transactionsFlags = new Dictionary<TransactionDTO, DataFlag>();
        public EBankModel(IEBankPersistence persistence)
        {
            IsUserLoggedIn = false;
            _persistence = persistence;
        }
        public IReadOnlyList<BankAccountsDTO> BankAccounts
        {
            get { return _bankAccounts; }
        }
        public IReadOnlyList<TransactionDTO> Transactions
        {
            get { return _transactions; }
        }
        public Boolean IsUserLoggedIn { get; private set; }
        public event EventHandler<BankAccountsEventArgs> BankAccountsChanged;
        public void LockBankAccount(BankAccountsDTO bankAccounts)
        {
            if (bankAccounts == null)
                throw new ArgumentNullException(nameof(bankAccounts));

            // keresés azonosító alapján
            BankAccountsDTO bankAccountsToLock = _bankAccounts.FirstOrDefault(b => b.ID == bankAccounts.ID);

            if (bankAccountsToLock == null)
                throw new ArgumentException("The building does not exist.", nameof(bankAccounts));

            bankAccountsToLock.isLocked = !bankAccountsToLock.isLocked;

            // külön kezeljük, ha egy adat újonnan hozzávett (ekkor nem kell törölni a szerverről)
            if (_bankAccountsFlags.ContainsKey(bankAccountsToLock) && _bankAccountsFlags[bankAccountsToLock] == DataFlag.Create)
                _bankAccountsFlags[bankAccountsToLock] = DataFlag.Create;
            else
                _bankAccountsFlags[bankAccountsToLock] = DataFlag.Update;

            OnAccountChanged(bankAccounts.ID);
        }


        public void AddIn(TransactionDTO transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            transaction.ID = (_transactions.Count > 0 ? _transactions.Max(b => b.ID) : 0) + 1; 
            _transactionsFlags.Add(transaction, DataFlag.Create);
            _transactions.Add(transaction);
            BankAccountsDTO bankacc = _bankAccounts.FirstOrDefault(f => f.ID == transaction.AccountID);
            bankacc.Balance = bankacc.Balance + transaction.TransactionValue;
            if (_bankAccountsFlags.ContainsKey(bankacc) && _bankAccountsFlags[bankacc] == DataFlag.Create)
                _bankAccountsFlags[bankacc] = DataFlag.Create;
            else
                _bankAccountsFlags[bankacc] = DataFlag.Update;


            OnAccountChanged(bankacc.ID);
        }
        public void TakeOut(TransactionDTO transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            transaction.ID = (_transactions.Count > 0 ? _transactions.Max(b => b.ID) : 0) + 1; 
            _transactionsFlags.Add(transaction, DataFlag.Create);
            _transactions.Add(transaction);
            BankAccountsDTO bankacc = _bankAccounts.FirstOrDefault(f => f.ID == transaction.AccountID);
            bankacc.Balance = bankacc.Balance - transaction.TransactionValue;
            if (_bankAccountsFlags.ContainsKey(bankacc) && _bankAccountsFlags[bankacc] == DataFlag.Create)
                _bankAccountsFlags[bankacc] = DataFlag.Create;
            else
                _bankAccountsFlags[bankacc] = DataFlag.Update;


            OnAccountChanged(bankacc.ID);
        }
        public void Tran(TransactionDTO transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            transaction.ID = (_transactions.Count > 0 ? _transactions.Max(b => b.ID) : 0) + 1;
            _transactionsFlags.Add(transaction, DataFlag.Create);
            _transactions.Add(transaction);
            BankAccountsDTO bankacc = _bankAccounts.FirstOrDefault(f => f.ID == transaction.AccountID);
            bankacc.Balance = bankacc.Balance - transaction.TransactionValue;
            if (_bankAccountsFlags.ContainsKey(bankacc) && _bankAccountsFlags[bankacc] == DataFlag.Create)
                _bankAccountsFlags[bankacc] = DataFlag.Create;
            else
                _bankAccountsFlags[bankacc] = DataFlag.Update;


            OnAccountChanged(bankacc.ID);


            BankAccountsDTO recbankacc = _bankAccounts.FirstOrDefault(f => f.AccountNumber == transaction.AccountNumberTo && f.ClientName == transaction.ReceiverName);

            if (recbankacc != null)
            {
                TransactionDTO rectran = new TransactionDTO
                {
                    TransactionValue = transaction.TransactionValue,
                    TransactionType = "Jóváírás",
                    Created = DateTime.Now,
                    AccountNumberFrom = bankacc.AccountNumber
                };
                _transactionsFlags.Add(rectran, DataFlag.Create);
                _transactions.Add(rectran);

                recbankacc.Balance = recbankacc.Balance + transaction.TransactionValue;
                if (_bankAccountsFlags.ContainsKey(recbankacc) && _bankAccountsFlags[recbankacc] == DataFlag.Create)
                    _bankAccountsFlags[recbankacc] = DataFlag.Create;
                else
                    _bankAccountsFlags[recbankacc] = DataFlag.Update;

                OnAccountChanged(recbankacc.ID);
            }
        }
        public async Task LoadAsync()
        {
            _bankAccounts = (await _persistence.ReadAccountsAsync()).ToList();
            
            _bankAccountsFlags = new Dictionary<BankAccountsDTO, DataFlag>();
        }

        public async Task SaveAsync()
        {
            List<BankAccountsDTO> bankAccountsToSave = _bankAccountsFlags.Keys.ToList();

            foreach (BankAccountsDTO bankAccounts in bankAccountsToSave)
            {
                Boolean result = true;
                
                switch (_bankAccountsFlags[bankAccounts])
                {
                    case DataFlag.Update:
                        result = await _persistence.UpdateAccountAsync(bankAccounts);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + _bankAccountsFlags[bankAccounts] + " failed on account " + bankAccounts.ID);
                
                _bankAccountsFlags.Remove(bankAccounts);
            }

            List<TransactionDTO> transactionsToSave = _transactionsFlags.Keys.ToList();
            foreach (TransactionDTO transaction in transactionsToSave)
            {
                Boolean result = true;
                
                switch (_transactionsFlags[transaction])
                {
                    case DataFlag.Create:
                        result = await _persistence.CreateTranAsync(transaction);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + _transactionsFlags[transaction] + " failed on account " + transaction.ID);
                
                _transactionsFlags.Remove(transaction);
            }
        }
        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            IsUserLoggedIn = await _persistence.LoginAsync(userName, userPassword);
            var tmp = IsUserLoggedIn;
            return IsUserLoggedIn;
        }
        public async Task<Boolean> LogoutAsync()
        {
            if (!IsUserLoggedIn)
                return true;

            IsUserLoggedIn = !(await _persistence.LogoutAsync());

            return IsUserLoggedIn;
        }

        private void OnAccountChanged(Int32 bankaccountsID)
        {
            if (BankAccountsChanged != null)
                BankAccountsChanged(this, new BankAccountsEventArgs { BankAccountsId = bankaccountsID });
        }
    }
}
