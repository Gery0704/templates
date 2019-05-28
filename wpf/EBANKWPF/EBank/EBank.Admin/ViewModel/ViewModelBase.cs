using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EBank.Admin.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<MessageEventArgs> MessageApplication;

        protected ViewModelBase() { }

        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnMessageApplication(String message)
        {
            if (MessageApplication != null)
                MessageApplication(this, new MessageEventArgs(message));
        }
    }
}
