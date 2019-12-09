using SchoolU_Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Administration.ViewModels
{
    public class SchoolYearViewModel : BaseViewModel, IDataErrorInfo
    {
        public SchoolYearViewModel()
        {
            SetupDataDirectory();
        }

        #region Private Properties
        private bool _isValid;
        private string _currentSchoolYear;
        private DateTime? _yearStartDate;
        private DateTime? _yearEndDate;
        private DateTime? _springStartDate;
        private DateTime? _springEndDate;
        private DateTime? _fallStartDate;
        private DateTime? _fallEndDate;
        private DateTime? _summerStartDate;
        private DateTime? _summerEndDate;
        private bool _isSaveEnabled;
        private bool _validateDates;
        private System.Collections.ObjectModel.ObservableCollection<Semester> _semesterCollection { get; set; } = new System.Collections.ObjectModel.ObservableCollection<Semester>();
        #endregion

        #region Public Properties
        public System.Collections.ObjectModel.ObservableCollection<Semester> SemesterCollection { get; set; } = new System.Collections.ObjectModel.ObservableCollection<Semester>();

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

        public string CurrentSchoolYear
        {
            get
            {
                return _currentSchoolYear;
            }
            set
            {
                _currentSchoolYear = value;
                propertyChanged(nameof(CurrentSchoolYear));
            }
        }

        public DateTime? YearStartDate
        {
            get
            {
                return _yearStartDate;
            }
            set
            {
                _yearStartDate = value;
                propertyChanged(nameof(YearStartDate));
            }
        }

        public DateTime? YearEndDate
        {
            get
            {
                return _yearEndDate;
            }
            set
            {
                _yearEndDate = value;
                propertyChanged(nameof(YearEndDate));
            }
        }

        public DateTime? SpringStartDate
        {
            get
            {
                return _springStartDate;
            }
            set
            {
                _springStartDate = value;
                propertyChanged(nameof(SpringStartDate));
            }
        }

        public DateTime? SpringEndDate
        {
            get
            {
                return _springEndDate;
            }
            set
            {
                _springEndDate = value;
                propertyChanged(nameof(SpringEndDate));
            }
        }

        public DateTime? FallStartDate
        {
            get
            {
                return _fallStartDate;
            }
            set
            {
                _fallStartDate = value;
                propertyChanged(nameof(FallStartDate));
            }
        }

        public DateTime? FallEndDate
        {
            get
            {
                return _fallEndDate;
            }
            set
            {
                _fallEndDate = value;
                propertyChanged(nameof(FallEndDate));
            }
        }

        public DateTime? SummerStartDate
        {
            get
            {
                return _summerStartDate;
            }
            set
            {
                _summerStartDate = value;
                propertyChanged(nameof(SummerStartDate));
            }
        }

        public DateTime? SummerEndDate
        {
            get
            {
                return _summerEndDate;
            }
            set
            {
                _summerEndDate = value;
                propertyChanged(nameof(SummerEndDate));
            }
        }

        public bool IsSaveEnabled
        {
            get
            {
                return _isSaveEnabled;
            }
            set
            {
                _isSaveEnabled = value;
                propertyChanged(nameof(IsSaveEnabled));
            }
        }

        #endregion

        public void ActivateSchoolYear()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    SchoolYear temp = context.SchoolYears.Where(i => i.Status == "A").SingleOrDefault();
                    if (temp != null)
                    {
                        temp.Status = "I";
                        context.Entry(temp).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                    return;
      
                }
            }
            catch(Exception ex)
            {

            }
        }

        public void SaveYear()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();

                    var schoolYear = new SchoolYear { StartDate = YearStartDate, EndDate = YearEndDate, Status = "A" };
                    context.Entry(schoolYear).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public void CommitSemester()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    IList<Semester> semesterList = new List<Semester>();
                    int currentYearId = context.SchoolYears.Where(i => i.Status == "A").Select(i => i.SchoolYearId).Single();
                    var temp1 = new Semester() { StartDate = FallStartDate, EndDate = FallEndDate, SemesterDescription = "Fall", SchoolYearId= currentYearId };
                    var temp2 = new Semester() { StartDate = SpringStartDate, EndDate = SpringEndDate, SemesterDescription = "Spring", SchoolYearId = currentYearId };
                    var temp3 = new Semester() { StartDate = SummerStartDate, EndDate = SummerEndDate, SemesterDescription = "Summer", SchoolYearId = currentYearId };
                    semesterList.Add(temp1);
                    semesterList.Add(temp2);
                    semesterList.Add(temp3);

                    context.Semesters.AddRange(semesterList);
                    context.SaveChanges();
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;

            }

        }

        public void Save()
        {
            if (ValidateDates())
            {
                ActivateSchoolYear();
                SaveYear();
                CommitSemester();
            }
            else
            {
                MessageBox.Show("End date cannot be before start date. Please retry.");
                return;
            }
        }

        public void ClearFields()
        {
            YearStartDate = null;
            YearEndDate = null;
            SpringStartDate = null;
            SpringEndDate = null;
            SummerStartDate = null;
            SummerEndDate = null;
            FallStartDate = null;
            FallEndDate = null;
            CurrentSchoolYear = string.Empty;
        }

        public bool ValidateDates()
        {
            return (YearStartDate < YearEndDate) && (SpringStartDate < SpringEndDate) && (SummerStartDate < SummerEndDate) && (FallStartDate < FallEndDate);
        }

        #region IDataErrorInfo

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch(columnName)
                {
                    case "CurrentSchoolYear":
                        if (string.IsNullOrEmpty(CurrentSchoolYear)) 
                        {
                            error = "Start date is required.";
                        }
                        break;
                    case "YearStartDate":
                        if(YearStartDate == null)
                        {
                            error = "Start date is required.";
                        }
                        break;
                    case "YearEndDate":
                        if (YearEndDate == null)
                        {
                            error = "End date is required.";
                        }
                        break;
                    case "SpringStartDate":
                        if (SpringStartDate == null)
                        {
                            error = "Start date is required.";
                        }
                        break;
                    case "SpringEndDate":
                        if (SpringEndDate == null)
                        {
                            error = "End date is required.";
                        }
                        break;
                    case "SummerStartDate":
                        if (SummerStartDate == null)
                        {
                            error = "Start date is required.";
                        }
                        break;
                    case "SummerEndDate":
                        if (SummerEndDate == null)
                        {
                            error = "End date is required.";
                        }
                        break;
                    case "FallStartDate":
                        if (FallStartDate == null)
                        {
                            error = "Start date is required.";
                        }
                        break;
                    case "FallEndDate":
                        if (FallEndDate == null)
                        {
                            error = "End date is required.";
                        }
                        break;
                }
                IsSaveEnabled = (!string.IsNullOrEmpty(CurrentSchoolYear) 
                                 && SpringStartDate != null 
                                 && SpringEndDate != null 
                                 && SummerStartDate != null 
                                 && SummerEndDate != null 
                                 && FallStartDate != null 
                                 && FallEndDate != null);
                return error;

            }
        }


        #endregion
    
    }
}
