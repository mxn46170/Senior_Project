using SchoolU_Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UserLogin.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Contructors
        public MainWindowViewModel()
        {
            SetupDataDirectory();
        }

        #endregion

        #region Private Fields
        private string _username;
        private string _password;

        #endregion

        #region Public Properties
        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                propertyChanged(nameof(UserName));
            }
        }

        public string Password
        {
            private get
            {
                return _password;
            }
            set
            {
                _password = value;
                propertyChanged(nameof(Password));
            }
        }

        #endregion

        #region Public Methods

        public void ValidateUserLogin(string pwd)
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    var admin = context.Admins.Where(i => i.Username == UserName && i.Password == pwd).SingleOrDefault();
                    var student = context.Students.Where(i => i.StudentUserName == UserName && i.StudentPassword == pwd).SingleOrDefault();
                    var professor = context.Professors.Where(i => i.ProfessorUserName == UserName && i.ProfessorPassword == pwd).SingleOrDefault();
                    if (admin != null)
                    {
                        Process.Start("Administration.exe");
                    }
                    else if (student != null)
                    {
                        Process.Start("SchoolApp_Student.exe");
                    }
                    else if (professor != null)
                    {
                        Process.Start("SchoolApp_Professor.exe");
                    }
                    else
                    {
                        MessageBox.Show("Incorrect username or password", "Failed Login", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Application.Current.Shutdown();
                return;
            }
        }

        #endregion

    }
}
