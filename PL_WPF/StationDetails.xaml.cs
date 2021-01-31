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
using BLApi;
using BO;
namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for StationDetails.xaml
    /// </summary>
    public partial class StationDetails : Window
    {
        List<BusLine> lines;
        /// <summary>
        /// the progrem shows the lines that stoping in the 
        /// selected station
        /// </summary>
        /// <param name="_bl">the performance of the BL</param>
        /// <param name="_lines">list of the lines</param>
        public StationDetails(IBL _bl, IEnumerable<BusLine> _lines)
        {
            InitializeComponent();
            lines = _lines.ToList();
            list.ItemsSource = lines;
        }
    }
}
