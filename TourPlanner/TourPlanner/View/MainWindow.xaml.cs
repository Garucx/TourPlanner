using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using TourPlanner.ViewModel;

namespace TourPlanner.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new TourViewModel();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            // Testen 
            Model.database test = new Model.database();
            Model.Tour a = new Model.Tour("Test1", "Some Desc 1", "Test", "Test", "Test", 10f, 10, new System.Windows.Media.Imaging.BitmapImage(), "https:test");
            test.Create_new_Tour(a);
            Model.TourLog b = new Model.TourLog(1, DateTime.Now, "asdasd", 3, 4, 4);
            test.Create_Tour_Log(b);


        }
    }
}
