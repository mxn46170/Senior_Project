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
using Administration.ViewModels;

namespace Administration.Views
{
    /// <summary>
    /// Interaction logic for AddEditStudentsView.xaml
    /// </summary>
    public partial class AddEditStudentsView : UserControl
    {
        private readonly AddEditStudentsViewModel addEditStudentsViewModel = new AddEditStudentsViewModel();
        public AddEditStudentsView()
        {
            InitializeComponent();
            DataContext = addEditStudentsViewModel;
        }

        private void AddButtonClicked(object sender, RoutedEventArgs e)
        {
            addEditStudentsViewModel.CommitStudent();
            PwdBox.Clear();
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            addEditStudentsViewModel.Reset();
            PwdBox.Clear();
        }
    }

}
