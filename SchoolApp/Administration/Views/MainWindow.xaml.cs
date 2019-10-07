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
using static Administration.ViewModels.NavigationViewModel;

namespace Administration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NavigationViewModel nav = new NavigationViewModel();
        private readonly MainWindowViewModel mvm = new MainWindowViewModel();
        public MainWindow()
        {
            Thread.Sleep(1500);
            InitializeComponent();
            DataContext = nav;
        }

        private void ShutDown(object sender, RoutedEventArgs e)
        {
            mvm.ExitApp();
            //Application.Current.Shutdown();
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
    }
}
