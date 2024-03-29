﻿using Administration.ViewModels;
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
    /// Interaction logic for StudentSearchView.xaml
    /// </summary>
    public partial class StudentSearchView : UserControl
    {
        private readonly StudentSearchViewModel ssvm = new StudentSearchViewModel();
        public StudentSearchView()
        {
            InitializeComponent();
            DataContext = ssvm;
        }

        private void ClearFilters(object sender, RoutedEventArgs e)
        {
            ssvm.ClearFilters();
        }
    }
}
