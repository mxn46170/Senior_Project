using SchoolU_Database;
using System;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Administration.ViewModels
{
    public class EventsViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Constructor(s)
        public EventsViewModel()
        {
            IsValid = true;
            StartDate = DateTime.Now.Date;
            EndDate = null;
            IsDepartmentControlEnabled = true;
            SetupDataDirectory();
            PopulateStartAndEndTimes();
            PopulateAM_PM_Cbo();
            PopulateDepartmentCbo();
        }
        #endregion

        #region Private Fields

        private string _selectedTimeOfDay_EndTime;
        private bool _isDepartmentControlEnabled;
        private string _eventTitle;
        private string _eventDescription;
        private DateTime _startDate;
        private DateTime? _endDate;
        private bool _isSchoolWideEvent;
        private Department _selectedDepartment;
        private string _selectedTimeOfDay_StartTime;
        private bool _isValid;
        private string _selectedStartTime;
        private string _selectedEndTime;
        private ObservableCollection<string> _timeCollection { get; set; } = new ObservableCollection<string>();
        private ObservableCollection<Department> _departmentCollection { get; set; } = new ObservableCollection<Department>();
        private ObservableCollection<string> _amPM { get; set; } = new ObservableCollection<string>();
        #endregion

        #region Public Properties

        public ObservableCollection<string> TimeCollection { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<Department> DepartmentCollection { get; set; } = new ObservableCollection<Department>();

        public ObservableCollection<string> AMPM{ get; set; } = new ObservableCollection<string>();

        public string EventTitle
        {
            get
            {
                return _eventTitle;
            }
            set
            {
                _eventTitle = value;
                propertyChanged(nameof(EventTitle));
            }
        }

        public string EventDescription
        {
            get
            {
                return _eventDescription;
            }
            set
            {
                _eventDescription = value;
                propertyChanged(nameof(EventDescription));
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                propertyChanged(nameof(StartDate));
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                propertyChanged(nameof(EndDate));
            }
        }

        /// <summary>
        /// Selected Item for Time controls
        /// </summary>
        public string SelectedStartTime
        {
            get
            {
                return _selectedStartTime;
            }
            set
            {
                _selectedStartTime = value;
                propertyChanged(nameof(SelectedStartTime));
            }
        }

        public string SelectedEndTime
        {
            get
            {
                return _selectedEndTime;
            }
            set
            {
                _selectedEndTime = value;
                propertyChanged(nameof(SelectedEndTime));
            }
        }


        public bool IsSchoolWideEvent
        {
            get
            {
                return _isSchoolWideEvent;
            }
            set
            {
                _isSchoolWideEvent = value;
                SelectedDepartment = DepartmentCollection.ElementAt(0);
                IsDepartmentControlEnabled = !_isSchoolWideEvent;
                propertyChanged(nameof(IsSchoolWideEvent));
            }
        }

        /// <summary>
        /// Selected Department for the Interested Department control
        /// </summary>
        public Department SelectedDepartment
        {
            get
            {
                return _selectedDepartment;
            }
            set
            {
                _selectedDepartment = value;
                propertyChanged(nameof(SelectedDepartment));
            }
        }

            
        /// <summary>
        /// Selected Time of day: AM or PM for StartDate
        /// </summary>
        public string SelectedTimeOfDay_StartTime
        {
            get
            {
                return _selectedTimeOfDay_StartTime;
            }
            set
            {
                _selectedTimeOfDay_StartTime = value;
                propertyChanged(nameof(SelectedTimeOfDay_StartTime));
            }
        }

        /// <summary>
        /// Selected Time of day: AM or PM for EndDate
        /// </summary>
        public string SelectedTimeOfDay_EndTime
        {
            get
            {
                return _selectedTimeOfDay_EndTime;
            }
            set
            {
                _selectedTimeOfDay_EndTime = value;
                propertyChanged(nameof(SelectedTimeOfDay_EndTime));
            }
        }


        /// <summary>
        /// Disables Department's cbo if the even being added
        /// is a school wide event
        /// </summary>
        public bool IsDepartmentControlEnabled
        {
            get
            {
                return _isDepartmentControlEnabled;
            }
            set
            {
                _isDepartmentControlEnabled = value;
                propertyChanged(nameof(IsDepartmentControlEnabled));
            }
        }

        /// <summary>
        /// Enables the Save button when all required fields
        /// are populated
        /// </summary>
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

        #endregion

        #region Public Methods
        /// <summary>
        /// Populates the Time collection which is used to populate
        /// the Time cbo with leading zeros when appropriate
        /// </summary>
        public void PopulateStartAndEndTimes()
        {
            TimeCollection.Clear();
            string time = string.Empty;
            for (int i = 1; i < 13; i++)
            { 
                if (i < 10)
                {
                    time = "0" + i.ToString();
                    _timeCollection.Add(time);
                }
                else
                {
                    _timeCollection.Add(i.ToString());
                }
            }
            TimeCollection = _timeCollection;
            SelectedStartTime = TimeCollection.ElementAt(0);
            SelectedEndTime = TimeCollection.ElementAt(0);
        }

        /// <summary>
        /// Populates the Time of day dbo 
        /// with AM and PM respectively
        /// </summary>
        public void PopulateAM_PM_Cbo()
        {
            _amPM.Add("AM");
            _amPM.Add("PM");
            AMPM = _amPM;
            SelectedTimeOfDay_StartTime = AMPM.ElementAt(0);
            SelectedTimeOfDay_EndTime = AMPM.ElementAt(0);
        }

        /// <summary>
        /// Verifies that all required fields are populated
        /// </summary>
        /// <returns></returns>
        public bool IsSavable()
        {
            if (IsSchoolWideEvent) // save is enabled when end date is not filled out //check it out
            {
                return IsValid = (!string.IsNullOrWhiteSpace(EventTitle) && !string.IsNullOrWhiteSpace(EventDescription) && SelectedStartTime != null && SelectedEndTime != null);
            }
            else
            {
                return IsValid = (!string.IsNullOrWhiteSpace(EventTitle) && !string.IsNullOrWhiteSpace(EventDescription) && SelectedStartTime != null && SelectedEndTime != null && SelectedDepartment != DepartmentCollection.ElementAt(0));
            }
        }

        /// <summary>
        /// Verifies that the selected end date is not before
        /// the selected start date
        /// </summary>
        public bool ValidateEventDates()
        {
            if (StartDate != null && EndDate != null && StartDate > EndDate)
            {
                MessageBox.Show("End date cannot be before the start date", "Invalid Date", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Verifies that the selected time are valid e.g.
        /// that the end time is not before the start time
        /// </summary>
        public bool ValidateEventTimes()
        {
            // if both times are AM or if both times are PM
            if (SelectedTimeOfDay_StartTime == AMPM.ElementAt(1) && SelectedTimeOfDay_EndTime == AMPM.ElementAt(0))
            {
                MessageBox.Show("End time cannot be before the start time", "Invalid Time", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            else if ((SelectedTimeOfDay_StartTime == AMPM.ElementAt(0) && SelectedTimeOfDay_EndTime == AMPM.ElementAt(0)) ||
                 SelectedTimeOfDay_StartTime == AMPM.ElementAt(1) && SelectedTimeOfDay_EndTime == AMPM.ElementAt(1))
            {
                if (int.Parse(SelectedStartTime) > int.Parse(SelectedEndTime))
                {
                    MessageBox.Show("End time cannot be before the start time", "Invalid Time", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Saves all user input after validation
        /// </summary>
        public void Save()
        {
            if (ValidateEventDates() && ValidateEventTimes())
            {
                CommitNewEvent();
                ClearFields();
                MessageBox.Show("Event added!",string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return;
        }

        /// <summary>
        /// Sets all fields to their default values
        /// following the save event
        /// </summary>
        public void ClearFields()
        {
            SelectedDepartment = DepartmentCollection.ElementAt(0);
            EventTitle = string.Empty;
            EventDescription = string.Empty;
            StartDate = DateTime.Now;
            EndDate = null;
            SelectedStartTime = TimeCollection.ElementAt(0);
            SelectedEndTime = TimeCollection.ElementAt(0);
            IsSchoolWideEvent = true ? false : true;
            SelectedTimeOfDay_StartTime = AMPM.ElementAt(0);
            SelectedTimeOfDay_EndTime = AMPM.ElementAt(0);
        }

        /// <summary>
        /// Inserts all user input in the events screen into
        /// the database
        /// </summary>
        public void CommitNewEvent()
        {
            try
            {
                using(var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    int departmentId = 0;
                    if (SelectedDepartment != DepartmentCollection.ElementAt(0))
                    {
                        departmentId = context.Departments.Where(i => i.DepartmentName == SelectedDepartment.DepartmentName).Select(i => i.DepartmentId).Single();
                    }
                    var newEvent = new Event()
                    {
                        EventName = EventTitle,
                        EventDescription = EventDescription,
                        IsSchoolWideEvent = IsSchoolWideEvent,
                        EventDate = StartDate,
                        EndDate = EndDate,
                        IntrestedDepartment = departmentId,
                        EventStartTime = DateTime.ParseExact(SelectedStartTime + ":00", "HH:mm", CultureInfo.InvariantCulture), EventEndTime = DateTime.ParseExact(SelectedEndTime + ":00", "HH:mm", CultureInfo.InvariantCulture)
                    };
                    context.Entry(newEvent).State = EntityState.Added;
                    context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Populates the Department cbo with
        /// records from the database
        /// </summary>
        public void PopulateDepartmentCbo()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    _departmentCollection.Add(new Department { DepartmentName = "None"});
                    context.Database.Connection.Open();
                    IList<Department> departments = context.Departments.Where(i => i.DepartmentId != 0).ToList();
                    foreach(var dep in departments)
                    {
                        _departmentCollection.Add(new Department { DepartmentName = dep.DepartmentName, DepartmentAbbr = dep.DepartmentAbbr});
                    }
                    DepartmentCollection = _departmentCollection;
                    if (DepartmentCollection.Count == 1)
                    {
                        SelectedDepartment = DepartmentCollection.ElementAt(1);
                    }
                    else
                    {
                        SelectedDepartment = DepartmentCollection.ElementAt(0);
                    }
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        #endregion

        #region IDataErrorInfo
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "EventTitle":
                        if (string.IsNullOrWhiteSpace(EventTitle))
                        {
                            error = "Event title is required.";
                        }
                        break;
                    case "EventDescription":
                        if (string.IsNullOrWhiteSpace(EventDescription))
                        {
                            error = "Event Description is required.";
                        }
                        break;
                    case "StartDate":
                        if (StartDate == null)
                        {
                            error = "Start Date is required.";
                        }
                        break;
                    case "EndDate":
                        if (EndDate == null)
                        {
                            error = "End Date is required.";
                        }
                        break;
                    case "SelectedStartTime":
                        if (SelectedStartTime == null)
                        {
                            error = "Start time is required.";
                        }
                        break;
                    case "SelectedEndTime":
                        if (SelectedEndTime == null)
                        {
                            error = "End time is required.";
                        }
                        break;
                    case "SelectedDepartment":
                        if (SelectedDepartment == null)
                        {
                            error = "Department is required.";
                        }
                        break;
                }
                IsSavable();
                return error;
            }
        }

        #endregion

    }
}
