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
    /// Interaction logic for SchoolYearView.xaml
    /// </summary>
    public partial class SchoolYearView : UserControl
    {
        private readonly ViewModels.SchoolYearViewModel syvm = new ViewModels.SchoolYearViewModel();

        public SchoolYearView()
        {
            InitializeComponent();
            DataContext = syvm;
        }

        private void SaveYear(object sender, RoutedEventArgs e)
        {
            syvm.Save();
            syvm.ClearFields();
        }

        private void ClearFields(object sender, RoutedEventArgs e)
        {
            syvm.ClearFields();
        }
    }
}
