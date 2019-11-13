using SchoolU_Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Administration.ViewModels
{
    public class CoursesViewModel : BaseViewModel, IDataErrorInfo // add edit and delete functionality // add multi-select functionality
    {
        #region Constructor(s)
        public CoursesViewModel()
        {
            SetupDataDirectory();
            PopulateAvailableCourses();
            PopulateDepartmentsCbo();
            Edit_SaveLabel = "Edit";
        }
        #endregion

        #region Private Fields
        private string _courseName;
        private Department _selectedDepartment;
        private Course _selectedCourse;
        private bool _isValid;
        private bool _isDeleteEnabled;
        private bool _isEditEnabled;
        private bool _iAddButtonEnabled;
        private bool _isRemoveButtonEnabled;
        private string _edit_SaveLabel;
        private ObservableCollection<Course> _availableCourseCollection { get; set; }  = new ObservableCollection<Course>();
        private ObservableCollection<Course> _selectedCourseCollection{ get; set; } = new ObservableCollection<Course>();
        private ObservableCollection<Department> _departmentCollection { get; set; } = new ObservableCollection<Department>();

        #endregion

        #region Public Properties

        /// <summary>
        ///  Enable and disables the Edit button
        /// </summary>
        public bool IsEditEnabled
        {
            get
            {
                return _isEditEnabled;
            }
            set
            {
                _isEditEnabled = SelectedCourse != null;
                propertyChanged(nameof(IsEditEnabled));
            }
        }

        /// <summary>
        /// Enable and disables the Delete button
        /// </summary>
        public bool IsDeleteEnabled
        {
            get
            {
                return _isDeleteEnabled;
            }
            set
            {
                _isDeleteEnabled = SelectedCourse != null;
                propertyChanged(nameof(IsDeleteEnabled));
            }
        }

        /// <summary>
        /// When editing a course this changes 
        /// the label from "Edit" to "Save" 
        /// </summary>
        public string Edit_SaveLabel
        {
            get
            {
                return _edit_SaveLabel;
            }
            set
            {
                _edit_SaveLabel = value;
                propertyChanged(nameof(Edit_SaveLabel));
            }
        }

        /// <summary>
        /// Contains the list of all courses retrieve 
        /// from the database
        /// </summary>
        public ObservableCollection<Course> AvailableCourseCollection
        {
            get
            {
                return _availableCourseCollection;
            }
            set
            {
                _availableCourseCollection = value;
                propertyChanged(nameof(AvailableCourseCollection));
            }
        }

        /// <summary>
        /// Contains list of all courses selected to be pre-reqs
        /// </summary>
        public ObservableCollection<Course> SelectedCourseCollection
        {
            get
            {
                return _selectedCourseCollection;
            }
            set
            {
                _selectedCourseCollection = value;
                propertyChanged(nameof(SelectedCourseCollection));
            }
        }

        public ObservableCollection<Department> DepartmentCollection { get; set; } = new ObservableCollection<Department>();

        /// <summary>
        /// Enables or disables the ">" button
        /// </summary>
        public bool IsAddButtonEnabled
        {
            get
            {
                return _iAddButtonEnabled;
            }
            set
            {
                _iAddButtonEnabled = value;
                propertyChanged(nameof(IsAddButtonEnabled));
            }
        }

        /// <summary>
        /// Enables or disables the "<" button
        /// </summary>
        public bool IsRemoveButtonEnabled
        {
            get
            {
                return _isRemoveButtonEnabled;
            }
            set
            {
                _isRemoveButtonEnabled = value;
                propertyChanged(nameof(IsRemoveButtonEnabled));
            }
        }

        /// <summary>
        /// Enables or disables the Add button
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

        /// <summary>
        /// Stores the course name
        /// </summary>
        public string CourseName
        {
            get 
            {
                return _courseName; 
            }
            set 
            { 
                _courseName = value;
                propertyChanged(nameof(CourseName));
            }
        }

        /// <summary>
        /// Stores the selected Department
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
        /// Stores the selected Course
        /// </summary>
        public Course SelectedCourse
        {
            get
            {
                return _selectedCourse;
            }
            set
            {
                _selectedCourse = value;
                IsEditEnabled = true;
                IsDeleteEnabled = true;
                propertyChanged(nameof(SelectedCourse));
            }
        }
        #endregion


        #region Public Methods

        //public void EditOrSave() //incomplete //not called in xaml.cs, should be when done
        //{
        //    if (Edit_SaveLabel == "Edit")
        //    {
        //        // There's an edit mode on EF
        //        EditCourse();
        //        //PopulateSelectedCoursesOnEdit();
        //        IsValid = false;
        //    }
        //    else if (Edit_SaveLabel == "Save")
        //    {
        //        UpdatePreReqs();
        //    }
        //}


        //public void SaveEdits() //incomplete
        //{
        //    CommitCourse();
        //    EditOrSave();
        //}

        /// <summary>
        /// Populates the fields with the appropriate values
        /// stored in the database for that selected course
        /// </summary>
        //public void EditCourse()
        //{
        //    Edit_SaveLabel = "Save";
        //    try
        //    {
        //        if (SelectedCourse != null)
        //        {
        //            using (var context = new SchoolU_DBEntities())
        //            {
        //                Course currentlySelectedCourse = context.Courses.Where(i => i.CourseName == SelectedCourse.CourseName).Single();

        //                CourseName = currentlySelectedCourse.CourseName;
        //                SelectedDepartment = DepartmentCollection.Where(i => i.DepartmentName == currentlySelectedCourse.Department.DepartmentName).Single();

        //                // Gets list of pre-req Ids which are courseIds in the database
        //                IList<PreRequisite> preReqs = context.PreRequisites.Where(i => i.CourseId == currentlySelectedCourse.CourseId).ToList();

        //                IList<PreRequisite> allPreReqs = context.PreRequisites.Where(i => i.CourseId != 0).ToList();
        //                IList<Course> allCourses = context.Courses.Where(i => i.CourseId != 0).ToList();

        //                IList<Course> courseObjects = (from course in allCourses join prerq in allPreReqs on course.CourseId equals prerq.PrereqId select course).ToList();

        //                if (courseObjects.Any())
        //                {
        //                    IList<Course> preReqCourse = new List<Course>();

        //                    foreach (var item in courseObjects)
        //                    {
        //                        preReqCourse.Add(item);
        //                    }

        //                    foreach (var item in preReqCourse)
        //                    {
        //                        SelectedCourseCollection.Add(item);
        //                        AvailableCourseCollection.Remove(item);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        //MessageBox.Show(ex.Message);
        //        return;
        //    }
        //}

        /// <summary>
        /// Deletes and adds the new pre-reqs unless no 
        /// changes were made to the pre-reqs
        /// </summary>
        //public void UpdatePreReqs()
        //{
        //    DeletePreReqs();
        //    CommitPreReqs();
        //}

        /// <summary>
        /// Reset all the fields to 
        /// their default values
        /// </summary>
        public void ClearFields()
        {
            CourseName = string.Empty;
            SelectedDepartment = DepartmentCollection.ElementAt(0);
            SelectedCourseCollection.Clear();
            AvailableCourseCollection.Clear();
            PopulateAvailableCourses();
            Edit_SaveLabel = "Edit";
        }

        /// <summary>
        /// Populates the Department cbo with
        /// records from the database
        /// </summary>
        public void PopulateDepartmentsCbo() //create a helper class. This method is created here and in Events ViewModel
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    _departmentCollection.Add(new Department { DepartmentName = "None" });
                    context.Database.Connection.Open();
                    IList<Department> departments = context.Departments.Where(i => i.DepartmentId != 0).ToList();
                    foreach (var dep in departments)
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
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Removes the selected course from the available courses
        /// list and adds to the pre-requisites list
        /// </summary>
        public void AddCourseAsPrerequisite()
        {
            if (SelectedCourse != null && !_selectedCourseCollection.Contains(SelectedCourse))
            {
                _selectedCourseCollection.Add(SelectedCourse);
                SelectedCourseCollection = _selectedCourseCollection;
                _availableCourseCollection.Remove(SelectedCourse);
                AvailableCourseCollection = _availableCourseCollection;
            }
        }

        /// <summary>
        /// Removes an added pre-requisite and adds it
        /// back to the available courses list
        /// </summary>
        public void RemovePreRequisite()
        {
            if (SelectedCourse != null && !_availableCourseCollection.Contains(SelectedCourse))
            {
                _availableCourseCollection.Add(SelectedCourse);
                AvailableCourseCollection = _availableCourseCollection;
                _selectedCourseCollection.Remove(SelectedCourse);
                SelectedCourseCollection = _selectedCourseCollection;
            }
        }


        /// <summary>
        /// Gets a list of course Ids based on the pre-req names
        /// currently in the pre-reqs list (SelectedCourseCollection)
        /// </summary>
        /// <returns></returns>
        public IList<int> GetPreReqIds()
        {
            IList<int> preReqIds = new List<int>();
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    if (SelectedCourseCollection.Any())
                    {
                        // Gets all the pre-req courses names
                        IList<string> pre_reqNames = SelectedCourseCollection.Where(i => i.CourseName != string.Empty).Select(i => i.CourseName).ToList();
                        // Gets all the course ids off of the courses names added to the pre-reqs list // pre-req = course ids
                        return preReqIds = context.Courses.Where(i => pre_reqNames.Contains(i.CourseName)).Select(i => i.CourseId).ToList();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            return null;
        }

        /// <summary>
        /// Checks whether the list of pre-reqs (at save/add time) is different
        /// from the pre-reqs currently in the database
        /// </summary>
        /// <returns></returns>
        public bool IsPreReqListDifferent()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    int courseAdded = context.Courses.Where(i => i.CourseName == CourseName).Select(i => i.CourseId).Single();

                    IList<int> preReqsInDB = context.PreRequisites.Where(i => i.CourseId == courseAdded).Select(i => i.PrereqId).ToList();

                    return !preReqsInDB.Equals(GetPreReqIds());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Commits the courses, that were moved to the 
        /// pre-reqs listview, to the preReqs table
        /// </summary>
        public void CommitPreReqs()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    // Gets all the courses names
                    IList<string> allCourseName = context.Courses.Where(i => i.CourseId != 0).Select(i => i.CourseName).ToList();
                    // Gets the course id for the course that was recently added
                    int courseAdded = context.Courses.Where(i => i.CourseName == CourseName).Select(i => i.CourseId).Single();

                    if (SelectedCourseCollection.Any())
                    {                     
                        // Stores the PreRequisite objects so they can be added all at the same time using the AddRange method
                        IList<PreRequisite> PreReqList = new List<PreRequisite>();

                        if (IsPreReqListDifferent())
                        {
                            for (int i = 0; i < GetPreReqIds().Count; i++)
                            {
                                var newPreReq = new PreRequisite()
                                {
                                    CourseId = courseAdded,
                                    PrereqId = GetPreReqIds().ElementAt(i)
                                };
                                PreReqList.Add(newPreReq);
                            }
                            context.PreRequisites.AddRange(PreReqList);
                        }
                    }
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
        /// Deletes the pre-reqs in the database so the
        /// new list of pre-reqs can be added
        /// </summary>
        //public void DeletePreReqs() //incomplete
        //{
        //    try
        //    {
        //        using (var context = new SchoolU_DBEntities())
        //        {
        //            // Gets all the courses names
        //            IList<string> allCourseName = context.Courses.Where(i => i.CourseId != 0).Select(i => i.CourseName).ToList();
        //            // Gets the course id for the course that was recently added
        //            int courseAdded = context.Courses.Where(i => i.CourseName == CourseName).Select(i => i.CourseId).Single();

        //            if (SelectedCourseCollection.Any())
        //            {
        //                // Gets all the pre-req courses names
        //                IList<string> pre_reqNames = SelectedCourseCollection.Where(i => i.CourseName != string.Empty).Select(i => i.CourseName).ToList();
        //                // Gets all the course ids off of the courses names added to the pre-reqs list // pre-req = course ids
        //                IList<int> preReqIds = context.Courses.Where(i => pre_reqNames.Contains(i.CourseName)).Select(i => i.CourseId).ToList();
        //                // Stores the PreRequisite objects so they can be added all at the same time using the AddRange method
        //                IList<PreRequisite> PreReqList = new List<PreRequisite>();

        //                context.PreRequisites.Attach();
        //                for (int i = 0; i < preReqIds.Count; i++)
        //                {
        //                    var newPreReq = new PreRequisite()
        //                    {
        //                        CourseId = courseAdded,
        //                        PrereqId = preReqIds.ElementAt(i)
        //                    };
        //                    PreReqList.Add(newPreReq);
        //                }
        //                context.PreRequisites.RemoveRange(PreReqList);
        //            }
        //            context.SaveChanges();
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return;
        //    }
        //}

        /// <summary>
        /// Commits all the information on the courses screen,
        /// except the preReqs, to the courses table in database
        /// </summary>
        public void CommitCourse()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    int departmentId = 0;

                    // get department
                    if (SelectedDepartment != DepartmentCollection.Where(i => i.DepartmentName == "None"))
                    {
                        departmentId = context.Departments.Where(i => i.DepartmentName == SelectedDepartment.DepartmentName).Select(i => i.DepartmentId).Single();
                    }

                    var newCourse = new Course()
                    {
                        CourseName = CourseName,
                        DepartmentId = departmentId
                    };
                    context.Entry(newCourse).State = (Edit_SaveLabel == "Edit") ? EntityState.Added : EntityState.Modified;
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
        /// Retrieves all the courses in the database and 
        /// populates the available courses listview
        /// </summary>
        public void PopulateAvailableCourses()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    AvailableCourseCollection.Clear();
                    IList<Course> allCourses = context.Courses.Where(i => i.CourseId != 0).ToList();
                    if (allCourses.Any())
                    {
                        foreach(var course in allCourses)
                        {
                            _availableCourseCollection.Add(course);
                        }
                        AvailableCourseCollection = _availableCourseCollection;

                        IsAddButtonEnabled = AvailableCourseCollection.Any();
                        IsRemoveButtonEnabled = AvailableCourseCollection.Any();
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
        /// Deletes the selected Course
        /// </summary>
        public void DeleteSelectedCourse()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    if (SelectedCourse != null)
                    {
                        Course course = context.Courses.Where(i => i.CourseName == SelectedCourse.CourseName).Single();
                        context.Entry(course).State = EntityState.Deleted;
                        AvailableCourseCollection.Remove(SelectedCourse);
                        context.SaveChanges();
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
        /// Deletes the pre-requisites for the selected course
        /// </summary>
        public void DeleteSelectedCourse_PreRequisites()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    if (SelectedCourse != null)
                    {
                        // Gets course of Id of the selected course
                        int courseId = context.Courses.Where(i => i.CourseName == SelectedCourse.CourseName).Select(i => i.CourseId).Single();
                        // Gets the pre-req ids for the selected course
                        IList<int> PreReqIds = context.PreRequisites.Where(i => i.CourseId == courseId).Select(i => i.PrereqId).ToList();

                        if (PreReqIds.Any())
                        {
                            // Gets a list of PreRequisite objects associated with the selected course
                            IList<PreRequisite> PreReqs = (from pr in context.PreRequisites join c in context.Courses 
                                                           on pr.PrereqId equals c.CourseId 
                                                           where PreReqIds.Contains(pr.PrereqId) 
                                                           select pr).ToList();
                            context.PreRequisites.RemoveRange(PreReqs);
                            context.SaveChanges();
                        }
                        else
                        {
                            return;
                        }
                    }
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
                    case "CourseName":
                        if (string.IsNullOrWhiteSpace(CourseName))
                        {
                            error = "Course Name is required.";
                        }
                        break;
                    case "SelectedDepartment":
                        if (SelectedDepartment == null)
                        {
                            error = "Department selection is required.";
                        }
                        break;
                }
                IsValid = (!string.IsNullOrWhiteSpace(CourseName) && SelectedDepartment != DepartmentCollection.Where(i => i.DepartmentName == "None"));
                return error;
            }
        }

        #endregion
    }
}
