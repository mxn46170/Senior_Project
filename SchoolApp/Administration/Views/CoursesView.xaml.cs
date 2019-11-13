using Administration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Administration.Views
{
    /// <summary>
    /// Interaction logic for CoursesView.xaml
    /// </summary>
    public partial class CoursesView : UserControl
    {
        private readonly CoursesViewModel cvm = new CoursesViewModel();
        public CoursesView()
        {
            InitializeComponent();
            DataContext = cvm;
        }

        private void AddPre_requisite(object sender, RoutedEventArgs e)
        {
            cvm.AddCourseAsPrerequisite();
        }

        private void RemovePre_requisite(object sender, RoutedEventArgs e)
        {
            cvm.RemovePreRequisite();
        }

        private void CommitNewCourse(object sender, RoutedEventArgs e)
        {
            cvm.CommitCourse();
            cvm.CommitPreReqs();
            cvm.ClearFields();
        }

        private void CancelAllChanges(object sender, RoutedEventArgs e)
        {
            cvm.ClearFields();
        }

        private void Edit_SaveChanges(object sender, RoutedEventArgs e)
        {
            //cvm.SaveEdits();
        }

        private void DeleteCourse(object sender, RoutedEventArgs e)
        {
            cvm.DeleteSelectedCourse_PreRequisites();
            cvm.DeleteSelectedCourse();
        }
    }
}
