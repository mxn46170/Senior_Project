using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Administration.ViewModels
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
    }
}
