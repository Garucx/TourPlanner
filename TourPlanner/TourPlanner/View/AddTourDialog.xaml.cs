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

namespace TourPlanner.View
{
    /// <summary>
    /// Interaction logic for AddTourDialog.xaml
    /// </summary>
    public partial class AddTourDialog : Window
    {
        public string Start_Address { get { return tb_start_address.Text; } }
        public string Start_AreaCode { get { return tb_start_areacode.Text; } }
        public string Start_City { get { return tb_start_city.Text; } }
        public string Start_Country { get { return tb_start_state.Text; } }
        public string Dest_Address { get { return tb_dest_address.Text; } }
        public string Dest_AreaCode { get { return tb_dest_areacode.Text; } }
        public string Dest_City { get { return tb_dest_city.Text; } }
        public string Dest_Country { get { return tb_dest_state.Text; } }
        public AddTourDialog()
        {
            InitializeComponent();
        }

        private void b_finish_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
