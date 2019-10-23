using SchoolU_Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Administration.ViewModels
{
    public class DepartmentsSetupViewModel : BaseViewModel
    {
        #region Constructor

        public DepartmentsSetupViewModel()
        {
            SetupDataDirectory();
            AddDepartment();
        }
        #endregion

        #region Private Fields
        private ObservableCollection<Department> _departmentCollection { get; set; } = new ObservableCollection<Department>();
        
        private string _dpAbbr;
        private string _dpName;
        #endregion

        #region Public Properties
        public ObservableCollection<Department> DepartmentCollection { get; set; } = new ObservableCollection<Department>();

        public string DpName
        {
            get 
            { 
                return _dpName; 
            }
            set 
            {
                _dpName = value;
                propertyChanged(nameof(DpName));
            }
        }

        public string DpAbbr
        {
            get
            {
                return _dpAbbr;
            }
            set
            {
                _dpAbbr = value;
                propertyChanged(nameof(DpAbbr));
            }
        }


        #endregion

        #region Public Methods

        public void ClearFields()
        {
            DpName = string.Empty;
            DpAbbr = string.Empty;
        }

        public void CommitDepartment()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    var temp = new Department() { DepartmentName = DpName, DepartmentAbbr = DpAbbr };
                    context.Entry(temp).State = EntityState.Added;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        public void AddDepartment()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    DepartmentCollection.Clear();
                    IList<Department> allDepartments = context.Departments.Where(i => i.DepartmentId != 0).ToList();
                    foreach (var dep in allDepartments)
                    {
                        _departmentCollection.Add(new Department { DepartmentName = dep.DepartmentName, DepartmentAbbr = dep.DepartmentAbbr });
                    }
                    DepartmentCollection = _departmentCollection;
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }
       

        #endregion

        #region Private Methods


        #endregion
    }
}
