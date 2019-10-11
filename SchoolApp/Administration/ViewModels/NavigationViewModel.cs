using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Administration.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        #region Fields
        private object selectedViewModel;

        #endregion

        #region Constructors
        public NavigationViewModel()
        {
            SchoolSetupCommand = new BaseCommand(OpenSchoolSetupView);
            SchoolYearCommand = new BaseCommand(OpenSchoolYearView);
            ProfessorsCommand = new BaseCommand(OpenProfessorsView);
            DepartmentsSetupCommand = new BaseCommand(OpenDepartmentsSetupView);
            BuildingsSetupCommand = new BaseCommand(OpenBuildingsSetupView);
            MajorMinorCommand = new BaseCommand(OpenMajorMinorView);
            StudentsCommand = new BaseCommand(OpenStudentsView);
            CoursesCommand = new BaseCommand(OpenCoursesView);

        }

        #endregion

        #region Public Properties
        public ICommand SchoolSetupCommand { get; set; }
        public ICommand SchoolYearCommand { get; set; }
        public ICommand ProfessorsCommand { get; set; }
        public ICommand DepartmentsSetupCommand { get; set; }
        public ICommand BuildingsSetupCommand { get; set; }
        public ICommand MajorMinorCommand { get; set; }
        public ICommand StudentsCommand { get; set; }
        public ICommand CoursesCommand { get; set; }


        #endregion

        #region Public Methods
        public object SelectedViewModel
        {
            get
            {
                return selectedViewModel;
            }
            set
            {
                selectedViewModel = value;
                propertyChanged("SelectedViewModel");
            }
        }

        #endregion

        #region Private Methods
        private void OpenSchoolSetupView(object obj)
        {
            SelectedViewModel = new SchoolSetupViewModel();
        }

        private void OpenSchoolYearView(object obj)
        {
            SelectedViewModel = new SchoolYearViewModel();
        }

        private void OpenProfessorsView(object obj)
        {
            SelectedViewModel = new ProfessorsViewModel();
        }
        
        private void OpenDepartmentsSetupView(object obj)
        {
            SelectedViewModel = new DepartmentsSetupViewModel();
        }

        private void OpenBuildingsSetupView(object obj)
        {
            SelectedViewModel = new BuildingsSetupViewModel();
        }

        private void OpenMajorMinorView(object obj)
        {
            SelectedViewModel = new MajorMinorViewModel();
        }
        
        public void OpenStudentsView(object obj)
        {
            SelectedViewModel = new StudentsViewModel();
        }

        private void OpenCoursesView(object obj)
        {
            SelectedViewModel = new CoursesViewModel();
        }
        
        #endregion

        public class MainWindowViewModel
        {
            public void ExitApp()
            {
                var result = MessageBox.Show("Are you sure you want to Exit Administration?", "Exiting Administration", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
                return;
            }

            public void LogOut()
            {
                var result = MessageBox.Show("Are you sure you want to Log out?", "Logging out", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Process.Start("UserLogin.exe");
                    Application.Current.Shutdown();
                }
                return;
            }
        }

    }
}
