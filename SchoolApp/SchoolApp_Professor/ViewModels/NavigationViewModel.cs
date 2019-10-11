using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolApp_Professor.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        #region Fields
        private object selectedViewModel;

        #endregion

        #region Constructors
        public NavigationViewModel()
        {
            PersonalInformationCommand = new BaseCommand(OpenPersonalInformation);
            UsernamePasswordCommand = new BaseCommand(OpenUsernamePassword);
            EmailCommand = new BaseCommand(OpenEmail);
        }

        #endregion

        #region Public Properties
        public ICommand PersonalInformationCommand { get; set; }
        public ICommand UsernamePasswordCommand { get; set; }

        public ICommand EmailCommand { get; set; }

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
        private void OpenPersonalInformation(object obj)
        {
            SelectedViewModel = new PersonalInformationViewModel();
        }

        private void OpenUsernamePassword(object obj)
        {
            SelectedViewModel = new UsernamePasswordViewModel();
        }

        private void OpenEmail(object obj)
        {
            SelectedViewModel = new EmailViewModel();
        }

        #endregion

    }
}
