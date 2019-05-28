using EBank.Admin.Model;
using EBank.Admin.Persistence;
using EBank.Admin.View;
using EBank.Admin.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EBank.Admin
{ /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
    public partial class App : Application
    {
        private IEBankModel _model;
        private LoginViewModel _loginViewModel;
        private LoginWindow _loginView;
        private MainViewModel _mainViewModel;
        private MainWindow _mainView;
        private AddInWindow _addInView;
        private TakeOutWindow _takeOutView;
        private TranView _tranView;
        private OtherWindow _otherWindow;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
            Exit += new ExitEventHandler(App_Exit);
        }
        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new EBankModel(new EBankPersistence("http://localhost:14571/")); // megadjuk a szolgáltatás címét

            _loginViewModel = new LoginViewModel(_model);
            _loginViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _loginViewModel.LoginSuccess += new EventHandler(ViewModel_LoginSuccess);
            _loginViewModel.LoginFailed += new EventHandler(ViewModel_LoginFailed);

            _loginView = new LoginWindow();
            _loginView.DataContext = _loginViewModel;
            _loginView.Show();

        }

        public async void App_Exit(object sender, ExitEventArgs e)
        {
            if (_model.IsUserLoggedIn) 
            {
                await _model.LogoutAsync();
            }
        }
        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            _mainViewModel = new MainViewModel(_model);
            _mainViewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            _mainViewModel.AddInFinished += new EventHandler(MainViewModel_AddInFinished);
            _mainViewModel.AddInStarted += new EventHandler<BankAccountsEventArgs>(MainViewModel_AddInStarted);
            _mainViewModel.TakeOutStarted += new EventHandler<BankAccountsEventArgs>(MainViewModel_TakeOutStarted);
            _mainViewModel.TakeOutFinished += new EventHandler(MainViewModel_TakeOutFinished);
            _mainViewModel.TranStarted += new EventHandler<BankAccountsEventArgs>(MainViewModel_TranStarted);
            _mainViewModel.TranFinished += new EventHandler(MainViewModel_TranFinished);
            _mainViewModel.OtherStarted += new EventHandler<BankAccountsEventArgs>(MainViewModel_OtherStarted);
            _mainViewModel.BankAccountOtherFinished += new EventHandler(MainViewModel_OtherFinished);
            _mainViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);

            _mainView = new MainWindow();
            _mainView.DataContext = _mainViewModel;
            _mainView.Show();

            _loginView.Close();
        }
        private void MainViewModel_AddInStarted(object sender, BankAccountsEventArgs e)
        {
            _addInView = new AddInWindow();
            _addInView.DataContext = _mainViewModel;
            _addInView.Show();
        }
        private void MainViewModel_TakeOutStarted(object sender, BankAccountsEventArgs e)
        {
            _takeOutView = new TakeOutWindow();
            _takeOutView.DataContext = _mainViewModel;
            _takeOutView.Show();
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "EBank", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainViewModel_AddInFinished(object sender, EventArgs e)
        {
            _addInView.Close();
        }

        private void MainViewModel_TakeOutFinished(object sender, EventArgs e)
        {
            _takeOutView.Close();
        }
        private void MainViewModel_TranStarted(object sender, BankAccountsEventArgs e)
        {
            _tranView = new TranView();
            _tranView.DataContext = _mainViewModel;
            _tranView.Show();
        }

        private void MainViewModel_OtherFinished(object sender, EventArgs e)
        {
            _otherWindow.Close();
        }
        private void MainViewModel_OtherStarted(object sender, BankAccountsEventArgs e)
        {
            _otherWindow = new OtherWindow();
            _otherWindow.DataContext = _mainViewModel;
            _otherWindow.Show();
        }
        private void MainViewModel_TranFinished(object sender, EventArgs e)
        {
            _tranView.Close();
        }

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("A bejelentkezés sikertelen!", "EBank", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        private void ViewModel_ExitApplication(object sender, System.EventArgs e)
        {
            Shutdown();
        }
    }
}
