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
    public class UsersViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Constructor

        public UsersViewModel()
        {
            SetupDataDirectory();
            AddUser();
            IsValid = false;
        }
        #endregion

        #region Private Fields
        private ObservableCollection<Admin> _userCollection { get; set; } = new ObservableCollection<Admin>();

        private string _dpAbbr;
        private string _dpName;
        private bool _isValid;
        #endregion

        #region Public Properties
        public ObservableCollection<Admin> DepartmentCollection { get; set; } = new ObservableCollection<Admin>();

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
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
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
                IsValid = (!string.IsNullOrWhiteSpace(DpName) && !string.IsNullOrWhiteSpace(DpAbbr)) ? true : false;
                return error;
            }
        }
        #endregion
    }
}
