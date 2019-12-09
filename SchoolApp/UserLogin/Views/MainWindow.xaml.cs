using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserLogin.ViewModels;

namespace UserLogin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel mvm = new MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
            Username.Focus();
            DataContext = mvm;
        }

        private void ValidatePassword(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                mvm.ValidateUserLogin(PwdBox.Password);
            }
        }

        private void OnEnterBtnClick(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            mvm.ValidateUserLogin(PwdBox.Password);
        }
    }
}
