using SchoolU_Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace Administration.ViewModels
{
    public class MajorMinorViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Constructor
        public MajorMinorViewModel()
        {
            SetupDataDirectory();
            AddMajor();
            IsValid = false;
        }
        #endregion

        #region Private Fields
        private string _majorDesc;
        private string _majorAbbr;
        private bool _isValid;
        private ObservableCollection<Major> _majorCollection { get; set; } = new ObservableCollection<Major>();
        #endregion

        #region Public Properties

        public ObservableCollection<Major> MajorCollection { get; set; } = new ObservableCollection<Major>();

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

        public string MajorDesc
        {
            get
            {
                return _majorDesc;
            }
            set
            {
                _majorDesc = value;
                propertyChanged(nameof(MajorDesc));
            }
        }

        public string MajorAbbr
        {
            get
            {
                return _majorAbbr;
            }
            set
            {
                _majorAbbr = value;
                propertyChanged(nameof(MajorAbbr));
            }
        }

        #endregion

        #region Public Methods

        public void ClearFields()
        {
            MajorDesc = string.Empty;
            MajorAbbr = string.Empty;
        }

        public void CommitMajor()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    var temp = new Major { MajorDescription = MajorDesc, MajorAbbr = MajorAbbr };
                    context.Entry(temp).State = EntityState.Added;
                    context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }

        public void AddMajor()
        {
            try
            {
                using (var context = new SchoolU_DBEntities())
                {
                    context.Database.Connection.Open();
                    MajorCollection.Clear();
                    IList<Major> allMajors = context.Majors.Where(i => i.MajorId != 0).ToList();
                    foreach(var m in allMajors)
                    {
                        _majorCollection.Add(new Major { MajorDescription = m.MajorDescription, MajorAbbr = m.MajorAbbr });
                    }
                    MajorCollection = _majorCollection;
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
                    case "MajorDesc":
                        if ((string.IsNullOrWhiteSpace(MajorDesc) && !string.IsNullOrWhiteSpace(MajorAbbr)) || (string.IsNullOrWhiteSpace(MajorDesc) && string.IsNullOrWhiteSpace(MajorAbbr)))
                        {
                            error = "Major Description is required.";
                        }
                        break;
                    case "MajorAbbr":
                        if ((string.IsNullOrWhiteSpace(MajorAbbr) && !string.IsNullOrWhiteSpace(MajorDesc)) || (string.IsNullOrWhiteSpace(MajorAbbr) && string.IsNullOrWhiteSpace(MajorDesc)))
                        {
                            error = "Major Abbreviation is required.";
                        }
                        break;
                }
                IsValid = (!string.IsNullOrWhiteSpace(MajorDesc) && !string.IsNullOrWhiteSpace(MajorAbbr)) ? true : false;
                return error;
            }
        }
        #endregion


    }
}
