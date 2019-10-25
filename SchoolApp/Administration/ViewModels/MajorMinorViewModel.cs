using SchoolU_Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace Administration.ViewModels
{
    public class MajorMinorViewModel : BaseViewModel
    {
        #region Constructor
        public MajorMinorViewModel()
        {
            SetupDataDirectory();
            AddMajor();
        }
        #endregion

        #region Private Fields
        private string _majorDesc;
        private string _majorAbbr;
        private ObservableCollection<Major> _majorCollection { get; set; } = new ObservableCollection<Major>();
        #endregion

        #region Public Properties

        public ObservableCollection<Major> MajorCollection { get; set; } = new ObservableCollection<Major>();

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


    }
}
