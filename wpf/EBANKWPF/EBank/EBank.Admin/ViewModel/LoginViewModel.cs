using EBank.Admin.Model;
using EBank.Admin.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EBank.Admin.ViewModel
{

    public class LoginViewModel : ViewModelBase
    {
        private IEBankModel _model;

        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand LoginCommand { get; private set; }

        public String UserName { get; set; }

        public event EventHandler ExitApplication;

        public event EventHandler LoginSuccess;

        public event EventHandler LoginFailed;

        public LoginViewModel(IEBankModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _model = model;
            UserName = String.Empty;

            ExitCommand = new DelegateCommand(param => OnExitApplication());

            LoginCommand = new DelegateCommand(param => LoginAsync(param as PasswordBox));
        }

        private async void LoginAsync(PasswordBox passwordBox)
        {
            if (passwordBox == null)
                return;

            try
            {
                Boolean result = await _model.LoginAsync(UserName, passwordBox.Password);


                if (result)
                    OnLoginSuccess();
                else
                    OnLoginFailed();
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("Nincs kapcsolat a kiszolgálóval.");
            }
        }

        private void OnLoginSuccess()
        {
            if (LoginSuccess != null)
                LoginSuccess(this, EventArgs.Empty);
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }

        private void OnLoginFailed()
        {
            if (LoginFailed != null)
                LoginFailed(this, EventArgs.Empty);
        }

    }
}
