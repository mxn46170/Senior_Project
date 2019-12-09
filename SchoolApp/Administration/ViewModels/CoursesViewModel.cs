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
    public class CoursesViewModel : BaseViewModel, IDataErrorInfo
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
        private ObservableCollection<Course> _availableCourseCollection { get; set; } = new ObservableCollection<Course>();
        private ObservableCollection<Course> _selectedCourseCollection { get; set; } = new ObservableCollection<Course>();
        private ObservableCollection<Department> _departmentCollection { get; set; } = new ObservableCollection<Department>();

        #endregion

        #region Public Properties

        public override string ToString()
        {
            return CourseName;
        }

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
                _isEditEnabled = value;
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

        /// <summary>
        /// Collections of all departments
        /// </summary>
        public ObservableCollection<Department> DepartmentCollection
        {
            get
            {
                return _departmentCollection;
            }
            set
            {
                _departmentCollection = value;
                propertyChanged(nameof(DepartmentCollection));
            }
        }

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

        /// <summary>
        /// Keeps track of the old course name when
        /// in edit mode
        /// </summary>
        public string OldCourseName { get; set; }
        #endregion


        #region Public Methods

        /// <summary>
        /// When editing, changes the edit label to "Save"
        /// so when done editing, you can save
        /// </summary>
        public void EditOrSave()
        {            
            if (Edit_SaveLabel == "Edit")
            {
                PopulateFieldsOnEdit();
                IsValid = false;
                OldCourseName = CourseName;
            }
            else if (Edit_SaveLabel == "Save")
            {
                if (OldCourseName == CourseName && !IsPreReqListDifferent())
                {
                    MessageBox.Show("No Changes detected", "Edit", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    ClearFields();
                    IsEditEnabled = SelectedCourse != null;
                    return;
                }
                else if (OldCourseName != CourseName)
                {
                    CommitCourse();
                }
                // Updates the pre-reqs only if the pre-reqs are different
                // than the ones in the database
                if (!IsCourseItsOwnPreRequisite())
                {
                    if (IsPreReqListDifferent())
                    {
                        UpdatePreReqs();
                    }
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("A course cannot be it's own Pre-requisite", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                MessageBox.Show("Course Edits Saved", "Edit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Checks if the course in edit is added
        /// as it's own pre-requisite
        /// </summary>
        /// <returns></returns>
        public bool IsCourseItsOwnPreRequisite()
        {
            return GetPreReqIds().Contains(SelectedCourseCollection.Where(i => i.CourseName == CourseName).Select(i => i.CourseId).SingleOrDefault());
        }

        /// <summary>
        /// Checks if the selected department is different from
        /// the one stored in the database for the selected course
        /// </summary>
        /// <returns></returns>
        public bool IsSelectedDepartmentDifferent()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();

                    int selectedDepId = context.Departments.Where(i => i.DepartmentName == SelectedDepartment.DepartmentName).Select(i => i.DepartmentId).Single();
                    Course currentCourse = context.Courses.Where(i => i.CourseName == OldCourseName).Single();

                    return !(currentCourse.DepartmentId == selectedDepId);
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Populates the fields with the appropriate values
        /// stored in the database for that selected course
        /// </summary>
        public void PopulateFieldsOnEdit()
        {
            Edit_SaveLabel = "Save";
            try
            {
                if (SelectedCourse != null)
                {
                    using (var context = new SchoolU_DBEntities())
                    {
                        // Gets the course object based on the selected Department Name
                        Course currentlySelectedCourse = context.Courses.Where(i => i.CourseName == SelectedCourse.CourseName).Single();
                        // Sets the Course name
                        CourseName = currentlySelectedCourse.CourseName;
                        // Gets the deparment name for the selected Course
                        string departmentName= context.Departments.Where(i => i.DepartmentId == currentlySelectedCourse.DepartmentId).Select(i => i.DepartmentName).Single();
                        // Selects the associated department for the course
                        SelectedDepartment = DepartmentCollection.Where(i => i.DepartmentName == departmentName).Single();

                        // Gets all the preReq Ids associated with the selected course
                        IList<int> preReqIds = (from c in context.Courses 
                                                join pr in context.PreRequisites on c.CourseId equals pr.CourseId 
                                                where c.CourseId == currentlySelectedCourse.CourseId 
                                                select pr.PrereqId).ToList();

                        if (preReqIds.Any())
                        {
                            // Gets the course name based on the Pre-req Id
                            IList<Course> course_Name = context.Courses.Where(i => preReqIds.Contains(i.CourseId)).ToList();

                            foreach (var item in course_Name)
                            {
                                if (AvailableCourseCollection.Where(i => i.CourseId == item.CourseId).Any())
                                {
                                    // Had to remove item this way, because it expects the exact same object to be in the list, otherwise it won't remove it.
                                    AvailableCourseCollection.Remove(AvailableCourseCollection.Where(i => i.CourseId == item.CourseId).Single());
                                }
                                SelectedCourseCollection.Add(item);
                            }
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

        /// <summary>
        /// Deletes and adds the new pre-reqs
        /// </summary>
        public void UpdatePreReqs()
        {
            DeletePreReqs();
            CommitPreReqs();
        }

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
            IsEditEnabled = SelectedCourse != null;
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
                    DepartmentCollection.Add(new Department { DepartmentName = "None" });
                    context.Database.Connection.Open();
                    IList<Department> departments = context.Departments.Where(i => i.DepartmentId != 0).ToList();
                    foreach (var dep in departments)
                    {
                        DepartmentCollection.Add(new Department { DepartmentName = dep.DepartmentName, DepartmentAbbr = dep.DepartmentAbbr});
                    }
                    if (DepartmentCollection.Count < 3)
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
            IsEditEnabled = (Edit_SaveLabel == "Save") ? true : SelectedCourse != null;

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
            IsEditEnabled = (Edit_SaveLabel == "Save") ? true : SelectedCourse != null;

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
        /// Check if the list of pre-requisites is different in any way
        /// If so, then returns true
        /// </summary>
        /// <returns></returns>
        public bool IsPreReqListDifferent()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    // Gets the course object based on the selected Department Name
                    Course currentlySelectedCourse = context.Courses.Where(i => i.CourseName == CourseName).Single();
                    // Variable to keep track of difference in pre-req list
                    bool isDifferent = true;

                    IList<int> preReqIds = (from c in context.Courses
                                            join pr in context.PreRequisites on c.CourseId equals pr.CourseId
                                            where c.CourseId == currentlySelectedCourse.CourseId
                                            select pr.PrereqId).ToList();

                    if (preReqIds.Any())
                    {
                        // Gets the course name based on the Pre-req Id
                        IList<Course> course_Name = context.Courses.Where(i => preReqIds.Contains(i.CourseId)).ToList();

                        if (SelectedCourseCollection.Count() == preReqIds.Count())
                        {
                            foreach (var item in course_Name)
                            {
                                // IsDifferent is set to false when the list hasn't changed at all
                                if (SelectedCourseCollection.Where(i => i.CourseId == item.CourseId).Any())
                                {
                                    isDifferent = false;
                                }
                                // IsDifferent is set to true when the list is different in one or more ways
                                else
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    return isDifferent;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Deletes the pre-reqs in the database so the
        /// new list of pre-reqs can be added
        /// </summary>
        public void DeletePreReqs()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {

                    // Gets the course object based on the selected Department Name
                    int currentlySelectedCourse = context.Courses.Where(i => i.CourseName == CourseName).Select(i => i.CourseId).Single();

                    // Gets a list of pre-reqs where the course id is the course currently in edit
                    IList<PreRequisite> preReqIds = (from c in context.Courses
                                                     join pr in context.PreRequisites on c.CourseId equals pr.CourseId
                                                     where c.CourseId == currentlySelectedCourse
                                                     select pr).ToList();

                    if (preReqIds.Any())
                    {
                        // List to store the pre-req objects to delete
                        IList<PreRequisite> PreReqsToDelete = new List<PreRequisite>();

                        foreach (var preReq in preReqIds)
                        {
                            PreReqsToDelete.Add(preReq);
                        }

                        context.PreRequisites.RemoveRange(PreReqsToDelete);
                        context.SaveChanges();
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

                    // get department id
                    if (SelectedDepartment != DepartmentCollection.Where(i => i.DepartmentName == "None"))
                    {
                        departmentId = context.Departments.Where(i => i.DepartmentName == SelectedDepartment.DepartmentName).Select(i => i.DepartmentId).Single();
                    }

                    // if NOT editing, create a new course and save
                    if (Edit_SaveLabel == "Edit")
                    {
                        var newCourse = new Course()
                        {
                            CourseName = CourseName,
                            DepartmentId = departmentId
                        };
                        context.Entry(newCourse).State = EntityState.Added;
                    }
                    // If editing, and course name or deparment is changed, then set status as modified before saving
                    else
                    {
                        Course currentCourse = context.Courses.Where(i => i.CourseName == OldCourseName).Single();

                        if (IsSelectedDepartmentDifferent())
                        {
                            currentCourse.DepartmentId = departmentId;
                        }
                        currentCourse.CourseName = CourseName;
                        context.Entry(currentCourse).State = EntityState.Modified;
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
        /// Deletes the selected Items and 
        /// clears all fields
        /// </summary>
        public void DeleteSelectedItem()
        {
            DeleteSelectedCourse_PreRequisites();
            DeleteSelectedCourse();
            ClearFields();
            MessageBox.Show("Course has been deleted", "Delete", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                        // Removes the selected course from the Available courses list
                        // Then deletes it from the database
                        Course courseForDeletion = context.Courses.Where(i => i.CourseName == SelectedCourse.CourseName).Single();
                        context.Entry(courseForDeletion).State = EntityState.Deleted;
                        AvailableCourseCollection.Remove(courseForDeletion);
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
                        // Gets the selected course's course Id 
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
                        if (SelectedDepartment == null || SelectedDepartment.DepartmentName == "None")
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
