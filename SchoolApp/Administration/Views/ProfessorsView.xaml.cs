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
    /// Interaction logic for ProfessorsView.xaml
    /// </summary>
    public partial class ProfessorsView : UserControl
    {
        private readonly ProfessorsViewModel pwm = new ProfessorsViewModel();
        public ProfessorsView()
        {
            InitializeComponent();
            DataContext = pwm;
        }

        private void SaveProfessor(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
