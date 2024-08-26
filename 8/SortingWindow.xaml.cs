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
using System.Windows.Shapes;

namespace _8
{
    /// <summary>
    /// Логика взаимодействия для SortingWindow.xaml
    /// </summary>
    public partial class SortingWindow : Window
    {
        public SortingWindow()
        {
            InitializeComponent();
        }

        public bool GroupAscending { get; set; }
        public bool GroupDescending { get; set; }
        public bool CourseAscending { get; set; }
        public bool CourseDescending { get; set; }
        public bool AgeAscending { get; set; }
        public bool AgeDescending { get; set; }
        public bool IDAscending { get; set; }
        public bool IDDescending { get; set; }

        private void Sorting_btn_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)rbAsCourse.IsChecked)
            {
                CourseAscending = true;
            }
            else if ((bool)rbDeCourse.IsChecked)
            {
                CourseDescending = true;
            }

            if ((bool)rbAsGroup.IsChecked)
            {
                GroupAscending = true;
            }
            else if ((bool)rbDeGroup.IsChecked)
            {
                GroupDescending = true;
            }

            if ((bool)rbAsAge.IsChecked)
            {
                AgeAscending = true;
            }
            else if((bool)rbDeAge.IsChecked)
            {
                AgeDescending = true;
            }

            if ((bool)rbAsID.IsChecked)
            {
                IDAscending = true;
            }
            else if ((bool)rbDeID.IsChecked)
            {
                IDDescending = true;
            }
            Close();
        }
    }
}
