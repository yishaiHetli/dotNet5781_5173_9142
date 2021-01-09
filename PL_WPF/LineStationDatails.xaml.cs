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
    /// Interaction logic for LineStationDatails.xaml
    /// </summary>
    public partial class LineStationDatails : Window
    {
        IBL bl;
        List<LineStation> lines = new List<LineStation>();
        public LineStationDatails(IBL _bl, IEnumerable<LineStation> _lines)
        {
            bl = _bl;
            lines = _lines.ToList();
            InitializeComponent();
            list.ItemsSource = lines;
        }
    }
}