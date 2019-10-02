using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Administration.ViewModels
{
    public class NavigationViewModel : INotifyPropertyChanged
    {
        #region Fields
        private object selectedViewModel;

        #endregion

        #region Constructors
        public NavigationViewModel()
        {
            SchoolSetupCommand = new BaseCommand(OpenSchoolSetupView);
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

        //Do not remove the code below
        #region ICommand
        public class BaseCommand : ICommand
        {
            private Predicate<object> _canExecute;
            private Action<object> _method;
            public event EventHandler CanExecuteChanged;

            public BaseCommand(Action<object> method)
                : this(method, null)
            {
            }

            public BaseCommand(Action<object> method, Predicate<object> canExecute)
            {
                _method = method;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                if (_canExecute == null)
                {
                    return true;
                }

                return _canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _method.Invoke(parameter);
            }
        }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void propertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
