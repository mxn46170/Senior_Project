using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolApp_Student.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Protected Methods
        protected void SetupDataDirectory()
        {
            string _databaseLocation;
            _databaseLocation = @"SchoolApp";
            var path = Path.GetFullPath(_databaseLocation);
            var temp = path.Substring(0, path.Length - 30);
            var temp1 = temp + "\\SchoolU_Database";
            AppDomain.CurrentDomain.SetData("DataDirectory", temp1);
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void propertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region ICommand
        public class BaseCommand : ICommand
        {
            private Predicate<object> _canExecute;
            private Action<object> _method;
            public event EventHandler CanExecuteChanged;

            public BaseCommand(Action<object> method)
                : this(method, null)
            {
            }

            public BaseCommand(Action<object> method, Predicate<object> canExecute)
            {
                _method = method;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                if (_canExecute == null)
                {
                    return true;
                }

                return _canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _method.Invoke(parameter);
            }
        }
        #endregion
    }
}
