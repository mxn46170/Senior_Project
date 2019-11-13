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
    /// Interaction logic for EventsView.xaml
    /// </summary>
    public partial class EventsView : UserControl
    {
        private readonly EventsViewModel evm = new EventsViewModel();
        public EventsView()
        {
            InitializeComponent();
            DataContext = evm;
        }

        private void SaveEventScreen(object sender, RoutedEventArgs e)
        {
            evm.Save();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
        }
    }
}
