using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.ViewModels
{
    public class StudentInformationModel : BaseViewModel
    {
        public string SFirstName { get; set; }

        public string SLastName { get; set; }

        public string SMajor { get; set; }
 
        public string SstudentYear { get; set; }

    }
}
