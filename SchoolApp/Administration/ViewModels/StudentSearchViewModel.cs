using SchoolU_Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Administration.ViewModels
{

    /* Create List and method -> store the result of everything upfront, then check that list for each of the filters
     * With each filter, the comboboxes' option also changes
     * 
     select s.StudentFirstName, s.StudentLastName, m.MajorDescription, sy.StudentYearDescription 
     from Student s join StudentYear sy on s.StudentYearId = sy.StudentYearId
     join StudentMajor sm on s.StudentId = sm.StudentId
     join Major m on sm.MajorId = m.MajorId
         */
    public class StudentSearchViewModel : BaseViewModel
    {
        #region Constructor(s)
        public StudentSearchViewModel()
        {
            SetupDataDirectory();
            PopulateStudentsCbo();
            PopulateMajorCbo();
            PopulateStudentYear();
            PopulateResultView_NoFilters();
        }

        #endregion

        #region Private Fields
        private Student _selectedFirstName;
        private Student _selectedLastName;
        private Major _selectedMajor;
        private StudentYear _selectedStudentYear;
        private int _numberOfStudents;
        private bool _isClearEnabled;
        private ObservableCollection<Student> _studentCollection { get; set; } = new ObservableCollection<Student>();
        private ObservableCollection<Major> _majorCollection { get; set; } = new ObservableCollection<Major>();
        private ObservableCollection<StudentYear> _studentYearCollection { get; set; } = new ObservableCollection<StudentYear>();
        private ObservableCollection<StudentInformationModel> _studentInformationCollection { get; set; } = new ObservableCollection<StudentInformationModel>();
        private ObservableCollection<StudentInformationModel> _allStudentInformation { get; set; } = new ObservableCollection<StudentInformationModel>();
        #endregion

        #region Public Properties

        /// <summary>
        /// Contains all the information for all the students retrieved from the database.
        /// This list will the one used when filtering the data, so the student information
        /// won't have to be retrieved from the database each time.
        /// </summary>
        public ObservableCollection<StudentInformationModel> StudentInformationCollection
        {
            get
            {
                return _studentInformationCollection;
            }
            set
            {
                _studentInformationCollection = value;
                propertyChanged(nameof(StudentInformationCollection));
            }
        }

        /// <summary>
        /// Enables/Disables the "Clear Filters" button
        /// if any comboboxes have a selection selected
        /// </summary>
        public bool IsClearEnabled
        {
            get
            {
                return _isClearEnabled;
            }
            set
            {
                _isClearEnabled = value;
                propertyChanged(nameof(IsClearEnabled));
            }
        }

        public ObservableCollection<StudentInformationModel> AllStudentInformation
        {
            get
            {
                return _allStudentInformation;
            }
            set
            {
                _allStudentInformation = value;
                propertyChanged(nameof(AllStudentInformation));
            }
        }

        public ObservableCollection<Student> StudentCollection
        {
            get
            {
                return _studentCollection;
            }
            set
            {
                _studentCollection = value;
                propertyChanged(nameof(StudentCollection));
            }
        }

        public ObservableCollection<Major> MajorCollection
        {
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

        public int NumberOfStudents
        {
            get
            {
                return _numberOfStudents;
            }
            set
            {
                _numberOfStudents = value;
                propertyChanged(nameof(NumberOfStudents));
            }
        }

        public Student SelectedFirstName
        {
            get
            {
                return _selectedFirstName;
            }
            set
            {
                _selectedFirstName = value;
                propertyChanged(nameof(SelectedFirstName));
                PopulateResultView_NoFilters();
                FilterOnFirstName();
                if (!IsClearable())
                {
                    StudentInformationCollection.Clear();
                    PopulateResultView_NoFilters();
                }
            }
        }

        public Student SelectedLastName
        {
            get
            {
                return _selectedLastName;
            }
            set
            {
                _selectedLastName = value;
                propertyChanged(nameof(SelectedLastName));
                PopulateResultView_NoFilters(); //enables filtering when changing selection, but doesn't consider another filter at the same time
                FilterOnLastName();
                if (!IsClearable())
                {
                    StudentInformationCollection.Clear();
                    PopulateResultView_NoFilters();
                }
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
                PopulateResultView_NoFilters();
                FilterOnMajor();
                if (!IsClearable())
                {
                    StudentInformationCollection.Clear();
                    PopulateResultView_NoFilters();
                }
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
                PopulateResultView_NoFilters();
                FilterStudentYear();
                if (!IsClearable())
                {
                    StudentInformationCollection.Clear();
                    PopulateResultView_NoFilters();
                }
            }
        }

        #endregion


        #region Public Methods


        public void FilterOnFirstName()
        {
            // If there's a valid selection aka if something other than "None" or NULL is selected then filter
            if (IsClearable())
            {
                StudentInformationCollection.Clear();
                var studentInfoFN = AllStudentInformation.Where(i => i.SFirstName == SelectedFirstName.StudentFirstName).ToList();
                NumberOfStudents = studentInfoFN.Count();
                foreach (var info in studentInfoFN)
                {
                    StudentInformationCollection.Add(info);
                }
            }
        }

        public void FilterOnLastName()
        {
            // If there's a valid selection aka if something other than "None" or NULL is selected then filter 
            if (IsClearable())
            {
                StudentInformationCollection.Clear();
                var studentLN = AllStudentInformation.Where(i => i.SLastName == SelectedLastName.StudentLastName).ToList();
                NumberOfStudents = studentLN.Count();
                foreach (var info in studentLN)
                {
                    StudentInformationCollection.Add(info);
                }
            }
        }

        public void FilterOnMajor()
        {
            // If there's a valid selection aka if something other than "None" or NULL is selected then filter 
            if (IsClearable())
            {
                StudentInformationCollection.Clear();
                var studentInfoMajor = AllStudentInformation.Where(i => i.SMajor == SelectedMajor.MajorDescription).ToList();
                NumberOfStudents = studentInfoMajor.Count();
                foreach (var info in studentInfoMajor)
                {
                    StudentInformationCollection.Add(info);
                }
            }
        }

        public void FilterStudentYear()
        {
            // If there's a valid selection aka if something other than "None" or NULL is selected then filter 
            if (IsClearable())
            {
                StudentInformationCollection.Clear();
                var studentInfoStudentYear = AllStudentInformation.Where(i => i.SstudentYear == SelectedStudentYear.StudentYearDescription).ToList();
                NumberOfStudents = studentInfoStudentYear.Count();
                foreach (var info in studentInfoStudentYear)
                {
                    StudentInformationCollection.Add(info);
                }
            }
        }

        /// <summary>
        /// Enables/Disables "Clear Filters" button
        /// </summary>
        /// <returns></returns>
        public bool IsClearable()
        {
            return IsClearEnabled = ((SelectedFirstName != null && SelectedFirstName.StudentFirstName != StudentCollection.Where(i => i.StudentFirstName == "None").Select(i => i.StudentFirstName).Single()) ||
                                    (SelectedLastName != null && SelectedLastName.StudentFirstName != StudentCollection.Where(i => i.StudentLastName == "None").Select(i => i.StudentLastName).Single()) ||
                                    (SelectedMajor != null && SelectedMajor.MajorDescription != MajorCollection.Where(i => i.MajorDescription == "None").Select(i => i.MajorDescription).Single()) ||
                                    (SelectedStudentYear != null && SelectedStudentYear.StudentYearDescription != StudentYearCollection.Where(i => i.StudentYearDescription == "None").Select(i => i.StudentYearDescription).Single()));
        }

        /// <summary>
        /// Displays all the students with no filters applied
        /// Stores all student information in AllStudentInformation list
        /// this list is then used for the rest of the view lifetime.
        /// Only retrieve info from database once if AllStudentInformation is empty
        /// </summary>
        public void PopulateResultView_NoFilters()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    if (!AllStudentInformation.Any())
                    {
                        var allStudentRelatedInformation = (from s in context.Students
                                                            join sy in context.StudentYears on s.StudentYearId equals sy.StudentYearId
                                                            join sm in context.StudentMajors on s.StudentId equals sm.StudentId
                                                            join m in context.Majors on sm.MajorId equals m.MajorId
                                                            select new
                                                            {
                                                                s.StudentFirstName,
                                                                s.StudentLastName,
                                                                m.MajorDescription,
                                                                sy.StudentYearDescription
                                                            }).ToList();

                        NumberOfStudents = allStudentRelatedInformation.Count();

                        foreach (var item in allStudentRelatedInformation)
                        {
                            AllStudentInformation.Add(new StudentInformationModel
                            {
                                SFirstName = item.StudentFirstName,
                                SLastName = item.StudentLastName,
                                SMajor = item.MajorDescription,
                                SstudentYear = item.StudentYearDescription
                            });
                        }
                        foreach (var item in AllStudentInformation)
                        {
                            StudentInformationCollection.Add(item);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Populates the Student combobox
        /// </summary>
        public void PopulateStudentsCbo()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    StudentCollection.Clear();
                    StudentCollection.Add(new Student { StudentFirstName = "None", StudentLastName = "None"});
                    IList<Student> allStudentsFN = context.Students.Where(i => i.StudentId != 0).ToList();
                    foreach(var item in allStudentsFN)
                    {
                        StudentCollection.Add(item);
                    }
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Populate Majors combobox
        /// </summary>
        public void PopulateMajorCbo()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    MajorCollection.Clear();
                    MajorCollection.Add(new Major { MajorDescription = "None" });
                    IList<Major> allMajors = context.Majors.Where(i => i.MajorId != 0).ToList();
                    foreach(var item in allMajors)
                    {
                        MajorCollection.Add(item);
                    }
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Populate the Student year combobox
        /// </summary>
        public void PopulateStudentYear()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    StudentYearCollection.Clear();
                    StudentYearCollection.Add(new StudentYear { StudentYearDescription = "None" });
                    IList<StudentYear> allStudentYear = context.StudentYears.Where(i => i.StudentYearId != 0).ToList();
                    foreach(var item in allStudentYear)
                    {
                        StudentYearCollection.Add(item);
                    }
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Sets all comboboxes to their default value
        /// </summary>
        public void ClearFilters()
        {
            SelectedFirstName = StudentCollection.Where(i => i.StudentFirstName == "None").Single();
            SelectedLastName = StudentCollection.Where(i => i.StudentLastName == "None").Single();
            SelectedMajor = MajorCollection.Where(i => i.MajorDescription == "None").Single();
            SelectedStudentYear= StudentYearCollection.Where(i => i.StudentYearDescription == "None").Single();
            AllStudentInformation.Clear();
            PopulateResultView_NoFilters();
        }

        #endregion

    }
}
