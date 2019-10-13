using SchoolApp_Student.ViewModels;
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

namespace SchoolApp_Student.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void DisplayPersonalInfo(object sender, RoutedEventArgs e)
        {
            SettingsViews.Content = new PersonalInformationViewModel();
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            SettingsViews.Content = new UsernamePasswordViewModel();
        }
    }
}
