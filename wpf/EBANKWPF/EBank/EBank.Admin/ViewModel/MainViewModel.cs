using EBank.Admin.Model;
using EBank.Admin.Persistence;
using EBank.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EBank.Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEBankModel _model;
        private ObservableCollection<BankAccountsDTO> _bankAccounts;
        private ObservableCollection<TransactionDTO> _transactions;
        private BankAccountsDTO _selectedBankAccount;
        private TransactionDTO _selectedtransactions;
        private Boolean _isLoaded;

        public ObservableCollection<BankAccountsDTO> BankAccounts
        {
            get { return _bankAccounts; }
            private set
            {
                if (_bankAccounts != value)
                {
                    _bankAccounts = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<BankAccountsDTO> BankAccountsOther
        {
            get { return _bankAccounts; }
            private set
            {
                if (_bankAccounts != value)
                {
                    _bankAccounts = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<TransactionDTO> Transactions
        {
            get { return _transactions; }
            private set
            {
                if (_transactions != value)
                {
                    _transactions = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsLoaded
        {
            get { return _isLoaded; }
            private set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public BankAccountsDTO SelectedBankAccount
        {
            get { return _selectedBankAccount; }
            set
            {
                if (_selectedBankAccount != value)
                {
                    _selectedBankAccount = value;
                    OnPropertyChanged();
                }
            }
        }
        public TransactionDTO SelectedTransaction
        {
            get { return _selectedtransactions; }
            set
            {
                if (_selectedtransactions != value)
                {
                    _selectedtransactions = value;
                    OnPropertyChanged();
                }
            }
        }

        public BankAccountsDTO EditedBankAccount { get; private set; }
        public TransactionDTO EditedTransaction { get; private set; }

        public DelegateCommand LockCommand { get; private set; }
        
        public DelegateCommand SaveAddInCommand { get; private set; }
        public DelegateCommand AddInCommand { get; private set; }
        public DelegateCommand OtherCommand { get; private set; }
        public DelegateCommand TakeOutCommand { get; private set; }
        public DelegateCommand TranCommand { get; private set; }
        public DelegateCommand SaveTakeOutCommand { get; private set; }


        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand LoadCommand { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }

        public event EventHandler ExitApplication;

        public event EventHandler<BankAccountsEventArgs> AddInStarted;
        public event EventHandler AddInFinished;
        public event EventHandler<BankAccountsEventArgs> BankAccountOtherStarted;
        public event EventHandler BankAccountOtherFinished;
        public event EventHandler<BankAccountsEventArgs> TakeOutStarted;
        public event EventHandler TakeOutFinished;
        public event EventHandler<BankAccountsEventArgs> TranStarted;
        public event EventHandler TranFinished;
        public event EventHandler<BankAccountsEventArgs> OtherStarted;
        public DelegateCommand CancelAddInCommand { get; private set; }
        public DelegateCommand CancelTakeOutCommand { get; private set; }
        public DelegateCommand SaveTranCommand { get; private set; }
        public DelegateCommand CancelTranCommand { get; private set; }

        public MainViewModel(IEBankModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;
            _model.BankAccountsChanged += Model_BankAccountChanged;
            _isLoaded = false;

            AddInCommand = new DelegateCommand(param =>
            {
                EditedTransaction = new TransactionDTO(); 
                EditedTransaction.AccountID = (param as BankAccountsDTO).ID;
                OnAddInStarted((param as BankAccountsDTO).ID);
            });

            OtherCommand = new DelegateCommand(param =>
            {
                EditedTransaction = new TransactionDTO();
                EditedTransaction.AccountID = (param as BankAccountsDTO).ID;
                OnOtherStarted((param as BankAccountsDTO).ID);
            });
            TakeOutCommand = new DelegateCommand(param =>
            {
                EditedTransaction = new TransactionDTO();
                EditedTransaction.AccountID = (param as BankAccountsDTO).ID;
                OnTakeOutStarted((param as BankAccountsDTO).ID);
            });
            TranCommand = new DelegateCommand(param =>
            {
                EditedTransaction = new TransactionDTO();
                EditedTransaction.AccountID = (param as BankAccountsDTO).ID;
                OnTranStarted((param as BankAccountsDTO).ID);
            });
            LockCommand = new DelegateCommand(param => LockBankAccount(param as BankAccountsDTO));
            SaveAddInCommand = new DelegateCommand(param => SaveAddIn());
            SaveTakeOutCommand = new DelegateCommand(param => SaveTakeOut());
            CancelAddInCommand = new DelegateCommand(param => CancelAddIn());
            CancelTakeOutCommand = new DelegateCommand(param => CancelTakeOut());
            SaveTranCommand = new DelegateCommand(param => SaveTran());
            CancelTranCommand = new DelegateCommand(param => CancelTran());
            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
        }

        private void UpdateBuilding(BankAccountsDTO bankAccount)
        {
            if (bankAccount == null)
                return;

            EditedBankAccount = new BankAccountsDTO
            {
                ID = bankAccount.ID,
                ClientName = bankAccount.ClientName,
                AccountNumber = bankAccount.AccountNumber,
                Balance = bankAccount.Balance,
                Created = bankAccount.Created,
                isLocked = bankAccount.isLocked
            };

        }

        private void LockBankAccount(BankAccountsDTO bankAccount)
        {
            if (bankAccount == null || !BankAccounts.Contains(bankAccount))
                return;
            var result = MessageBox.Show("Biztos benne?", "Figyelmeztetés", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes;
            if (result)
            {
                _model.LockBankAccount(bankAccount);
            }
        }
         
        private void SaveAddIn()
        {
            // ellenőrzések
            if (String.IsNullOrEmpty(EditedTransaction.TransactionValue.ToString()))
            {
                OnMessageApplication("A mező üres");
                return;
            }

            if (BankAccounts.FirstOrDefault(b => b.ID == EditedTransaction.AccountID).isLocked)
            {
                OnMessageApplication("A számla lezárva");
                return;
            }
            EditedTransaction.Created = DateTime.Now;
            EditedTransaction.TransactionType = "Betét";
            Transactions.Add(EditedTransaction);
            SelectedTransaction = EditedTransaction;

            _model.AddIn(EditedTransaction);

            EditedTransaction = null;

            OnAddInFinished();
        }
        private void SaveTakeOut()
        {
            if (String.IsNullOrEmpty(EditedTransaction.TransactionValue.ToString()))
            {
                OnMessageApplication("A mező üres");
                return;
            }

            if (BankAccounts.FirstOrDefault(b => b.ID == EditedTransaction.AccountID).isLocked)
            {
                OnMessageApplication("A számla lezárva");
                return;
            }

            if (BankAccounts.FirstOrDefault(b => b.ID == EditedTransaction.AccountID).Balance < EditedTransaction.TransactionValue)
            {
                OnMessageApplication("A tranzakció összege nem haladhatja meg az egyenleget!");
                return;
            }

            EditedTransaction.Created = DateTime.Now;
            EditedTransaction.TransactionType = "Kivétel";
            Transactions.Add(EditedTransaction);
            SelectedTransaction = EditedTransaction;

            _model.TakeOut(EditedTransaction);

            EditedTransaction = null;

            OnTakeOutFinished();
        }

        private void SaveTran()
        {
            if (String.IsNullOrEmpty(EditedTransaction.TransactionValue.ToString()))
            {
                OnMessageApplication("Az összeg mező üres");
                return;
            }
            if (EditedTransaction.AccountNumberTo==null)
            {
                OnMessageApplication("A cél számlaszám üres");
                return;
            }
            if (EditedTransaction.ReceiverName==null)
            {
                OnMessageApplication("A fogadó neve üres");
                return;
            }

            if (BankAccounts.FirstOrDefault(b => b.ID == EditedTransaction.AccountID).isLocked)
            {
                OnMessageApplication("A számla lezárva");
                return;
            }

            if (BankAccounts.FirstOrDefault(b => b.ID == EditedTransaction.AccountID).Balance < EditedTransaction.TransactionValue)
            {
                OnMessageApplication("A tranzakció összege nem haladhatja meg az egyenleget!");
                return;
            }

            EditedTransaction.Created = DateTime.Now;
            EditedTransaction.TransactionType = "Terhelés";
            Transactions.Add(EditedTransaction);
            SelectedTransaction = EditedTransaction;

            _model.Tran(EditedTransaction);

            EditedTransaction = null;

            OnTranFinished();
        }


        private void CancelAddIn()
        {
            EditedBankAccount = null;
            EditedTransaction = null;
            OnAddInFinished();
        }
        private void CancelTakeOut()
        {
            EditedBankAccount = null;
            EditedTransaction = null;
            OnTakeOutFinished();
        }
        private void CancelTran()
        {
            EditedBankAccount = null;
            EditedTransaction = null;
            OnTranFinished();
        }
        private async void LoadAsync()
        {
            try
            {
                await _model.LoadAsync();
                BankAccounts = new ObservableCollection<BankAccountsDTO>(_model.BankAccounts);
                BankAccountsOther = new ObservableCollection<BankAccountsDTO>(_model.BankAccounts);
                Transactions = new ObservableCollection<TransactionDTO>(_model.Transactions);
                IsLoaded = true;
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A betöltés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        private async void SaveAsync()
        {
            try
            {
                await _model.SaveAsync();
                OnMessageApplication("A mentés sikeres!");
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A mentés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        private void Model_BankAccountChanged(object sender, BankAccountsEventArgs e)
        {
            Int32 index = BankAccounts.IndexOf(BankAccounts.FirstOrDefault(bankacc => bankacc.ID == e.BankAccountsId));
            BankAccounts.RemoveAt(index);
            BankAccounts.Insert(index, _model.BankAccounts[index]);

            SelectedBankAccount = BankAccounts[index]; 
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }

        private void OnAddInStarted(Int32 accountID)
        {
            if (AddInStarted != null)
                AddInStarted(this, new BankAccountsEventArgs { BankAccountsId = accountID });
        }
        private void OnAddInFinished()
        {
            if (AddInFinished != null)
                AddInFinished(this, EventArgs.Empty);
        }
        
        private void OnTakeOutStarted(Int32 accountID)
        {
            if (TakeOutStarted != null)
                TakeOutStarted(this, new BankAccountsEventArgs { BankAccountsId = accountID });
        }
        private void OnOtherStarted(Int32 accountID)
        {
            if (OtherStarted != null)
            {
                BankAccountsOther = new ObservableCollection<BankAccountsDTO>(_model.BankAccounts.Where(b => b.ID == accountID));
                OtherStarted(this, new BankAccountsEventArgs { BankAccountsId = accountID });
            }
        }
        private void OnTakeOutFinished()
        {
            if (TakeOutFinished != null)
                TakeOutFinished(this, EventArgs.Empty);
        }
        private void OnTranStarted(Int32 accountID)
        {
            if (TranStarted != null)
                TranStarted(this, new BankAccountsEventArgs { BankAccountsId = accountID });
        }
        private void OnTranFinished()
        {
            if (TranFinished != null)
                TranFinished(this, EventArgs.Empty);
        }

    }
}
