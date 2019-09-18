using Administration.Views;
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

namespace Administration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShutDown(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            //check if the current screen has any unsaved data -> output message to save or not?
        }

        private void DisplaySchoolSetupView(object sender, RoutedEventArgs e)
        {
            //Temporary code, this is not how we want to do this
            tmp.Visibility = Visibility.Visible;

            //Whenever the SchoolSetupView is displayed, we want the DepartmentsSetupView to be collapsed:
            tmp1.Visibility = Visibility.Collapsed;
        }

        private void DisplayDepartmentsSetupView(object sender, RoutedEventArgs e)
        {
            //Temporary code, this is not how we want to do this
            tmp1.Visibility = Visibility.Visible;

            //Whenever the DepartmentsSetupView is displayed, we want the SchoolSetupView to be collapsed:
            tmp.Visibility = Visibility.Collapsed;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            // Begin dragging the window
            DragMove();
        }
    }
}
