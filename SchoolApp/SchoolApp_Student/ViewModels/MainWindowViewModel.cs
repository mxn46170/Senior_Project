using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchoolApp_Student.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields

        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        public void ExitApp()
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Exiting", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
            return;
        }

        public void LogOut()
        {
            var result = MessageBox.Show("Are you sure you want to Log out?", "Logging out", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Process.Start("UserLogin.exe");
                Application.Current.Shutdown();
            }
            return;
        }

        #endregion

        #region Private Methods


        #endregion
    }
}
