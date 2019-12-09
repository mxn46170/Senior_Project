using Administration.ViewModels;
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

namespace Administration.Views
{
    /// <summary>
    /// Interaction logic for DepartmentsSetupView.xaml
    /// </summary>
    public partial class DepartmentsSetupView : UserControl
    {
        private readonly DepartmentsSetupViewModel dpvm = new DepartmentsSetupViewModel();
        public DepartmentsSetupView()
        {
            InitializeComponent();
            DataContext = dpvm;
        }

        private void AddDepartment(object sender, RoutedEventArgs e)
        {
            dpvm.CommitDepartment();
            dpvm.AddDepartment();
            dpvm.ClearFields();
        }

        private void DeleteDepartment(object sender, RoutedEventArgs e)
        {
            dpvm.DeleteDepartment();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            dpvm.ClearFields();
        }
    }
}
