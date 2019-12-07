using SchoolU_Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.ViewModels
{
    public class AddEditStudentsViewModel : BaseViewModel
    {
        #region Constructor
        public AddEditStudentsViewModel()
        {
            SetupDataDirectory();
            PopulateMajors();
            PopulateStudentYears();
            IsValid = false;
        }
        #endregion

        #region Private Fields
        private ObservableCollection<Major> _majorCollection { get; set; } = new ObservableCollection<Major>();
        private ObservableCollection<StudentYear> _studentYearCollection { get; set; } = new ObservableCollection<StudentYear>();
        private bool _isValid;
        private String _major;
        private Major _selectedMajor;
        private Major _selectedMinor;
        private StudentYear _selectedStudentYear;
        #endregion

        #region Public Properties
        public ObservableCollection<Major> MajorCollection {
            get
            {
                return _majorCollection;
            }
            set
            {
                _majorCollection = value;
                propertyChanged(nameof(MajorCollection));
            }
        }

        public ObservableCollection<StudentYear> StudentYearCollection
        {
            get
            {
                return _studentYearCollection;
            }
            set
            {
                _studentYearCollection = value;
                propertyChanged(nameof(StudentYearCollection));
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                propertyChanged(nameof(IsValid));
            }
        }

        public string Major
        {
            get
            {
                return _major;
            }
            set
            {
                _major = value;
                propertyChanged(nameof(Major));
            }
        }

        public StudentYear SelectedStudentYear
        {
            get
            {
                return _selectedStudentYear;
            }
            set
            {
                _selectedStudentYear = value;
                propertyChanged(nameof(SelectedStudentYear));
            }
        }

        public Major SelectedMajor
        {
            get
            {
                return _selectedMajor;
            }
            set
            {
                _selectedMajor = value;
                propertyChanged(nameof(SelectedMajor));
            }
        }

        public Major SelectedMinor
        {
            get
            {
                return _selectedMinor;
            }
            set
            {
                _selectedMinor = value;
                propertyChanged(nameof(SelectedMinor));
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Populates StudentYearCollection with all StudentYearDescription in the StudentYear table
        /// in the database
        /// </summary>
        /// <returns></returns>
        public void PopulateStudentYears()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    StudentYearCollection.Clear();
                    StudentYearCollection.Add(new StudentYear { StudentYearDescription = "None" });
                    var dbStudentYears = context.StudentYears.Where(i => i.StudentYearId != 0).ToList();

                    foreach (var m in dbStudentYears)
                    {
                        StudentYearCollection.Add(new StudentYear { StudentYearDescription = m.StudentYearDescription });
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Populates MajorCollection with all majors in the Major table in the database
        /// </summary>
        /// <returns></returns>
        public void PopulateMajors()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    MajorCollection.Clear();
                    IList<Major> allMajors = context.Majors.Where(i => i.MajorId != 0).ToList();
                    foreach (var m in allMajors)
                    {
                        _majorCollection.Add(new Major { MajorDescription = m.MajorDescription });
                    }
                    MajorCollection = _majorCollection;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion

        #region IDataErrorInfo

        #endregion
    }
}
