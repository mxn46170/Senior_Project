﻿using SchoolU_Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Administration.ViewModels
{
    public class DepartmentsSetupViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Constructor

        public DepartmentsSetupViewModel()
        {
            SetupDataDirectory();
            AddDepartment();
            IsValid = false;
            IsDeleteEnabled = false;
            IsEditEnabled = false;
        }
        #endregion

        #region Private Fields
        private ObservableCollection<Department> _departmentCollection { get; set; } = new ObservableCollection<Department>();
        
        private string _dpAbbr;
        private string _dpName;
        private bool _isValid;
        private bool _isDeleteEnabled;
        private bool _isEditEnabled;
        private Department _selectedDepartment;
        #endregion

        #region Public Properties
        public ObservableCollection<Department> DepartmentCollection { get; set; } = new ObservableCollection<Department>();

        public bool IsEditEnabled
        {
            get
            {
                return _isEditEnabled;
            }
            set
            {
                _isEditEnabled = SelectedDepartment != null;
                propertyChanged(nameof(IsEditEnabled));
            }
        }

        public bool IsDeleteEnabled
        {
            get
            {
                return _isDeleteEnabled;
            }
            set
            {
                _isDeleteEnabled = (SelectedDepartment != null);
                propertyChanged(nameof(IsDeleteEnabled));
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

        public Department SelectedDepartment
        {
            get
            {
                return _selectedDepartment;
            }
            set
            {
                _selectedDepartment = value;
                IsDeleteEnabled = true;
                IsEditEnabled = true;
                propertyChanged(nameof(SelectedDepartment));
            }
        }

        #endregion

        #region Public Methods

        public void ClearFields()
        {
            DpName = string.Empty;
            DpAbbr = string.Empty;
            SelectedDepartment = null;
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

        public void DeleteDepartment()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    if (SelectedDepartment != null)
                    {
                        Department getDepartmentInfo = context.Departments.Where(i => i.DepartmentName == SelectedDepartment.DepartmentName && i.DepartmentAbbr == SelectedDepartment.DepartmentAbbr).Single();
                        context.Entry(getDepartmentInfo).State = EntityState.Deleted;
                        context.SaveChanges();
                        DepartmentCollection.Remove(SelectedDepartment);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        #endregion

        #region Private Methods


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
                    case "DpName":
                        if (string.IsNullOrWhiteSpace(DpName))
                        {
                            error = "Department Name is required.";
                        }
                        break;
                    case "DpAbbr":
                        if (string.IsNullOrWhiteSpace(DpAbbr))
                        {
                            error = "Department Abbreviation is required.";
                        }
                        break;
                }
                IsValid = (!string.IsNullOrWhiteSpace(DpName) && !string.IsNullOrWhiteSpace(DpAbbr));
                return error;
            }
        }
        #endregion
    }
}
