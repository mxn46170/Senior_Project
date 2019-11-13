using Administration.ViewModels;
using Administration.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly MainWindowViewModel mvm = new MainWindowViewModel();
        public MainWindow()
        {
            //Thread.Sleep(1500);
            InitializeComponent();
            DataContext = mvm;
        }

        private void ShutDown(object sender, RoutedEventArgs e)
        {
            mvm.ExitApp();
            //check if the current screen has any unsaved data -> output message to save or not?
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            // Begin dragging the window
            DragMove();
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            mvm.LogOut();
        }

        #region Navigation
        private void DisplaySchoolInfoView(object sender, RoutedEventArgs e)
        {
            Main.Content = new SchoolSetupViewModel();
        }

        private void DisplaySchoolYearView(object sender, RoutedEventArgs e)
        {
            Main.Content = new SchoolYearViewModel();
        }

        private void DisplayBuildingRoomsView(object sender, RoutedEventArgs e)
        {
            Main.Content = new BuildingsSetupViewModel();
        }

        private void DisplayDepartmentsView(object sender, RoutedEventArgs e)
        {
            Main.Content = new DepartmentsSetupViewModel();
        }

        private void DisplayMajorsView(object sender, RoutedEventArgs e)
        {
            Main.Content = new MajorMinorViewModel();
        }

        private void DisplayProfessorsView(object sender, RoutedEventArgs e)
        {
            Main.Content = new ProfessorsViewModel();
        }

        private void DisplayCoursesView(object sender, RoutedEventArgs e)
        {
            Main.Content = new CoursesViewModel();
        }

        private void DisplayStudentsView(object sender, RoutedEventArgs e)
        {
            Main.Content = new StudentsViewModel();
        }

        private void DisplayEventsView(object sender, RoutedEventArgs e)
        {
            Main.Content = new EventsViewModel();
        }

        #endregion
    }
}
