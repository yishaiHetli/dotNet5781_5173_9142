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
using BL;
using System.Text.RegularExpressions;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for UpdatePairWin.xaml
    /// </summary>
    public partial class UpdatePairWin : Window
    {
        IBL bl;
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        int station1 = -1;
        int station2 = -1;
        double distance = 0;
        TimeSpan averageTime = TimeSpan.Zero;
        public UpdatePairWin(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            if (station1 > 0 && station2 > 0 && averageTime > TimeSpan.Zero && distance > 0)
            {
                try { bl.PairUpdate(station1, station2, distance, averageTime); }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "error");
                    next();
                    return;
                }
                this.Close();
            }
            else { MessageBox.Show("some value was missing"); next(); }
        }

        private void staBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//if the user press on enter
            {
                if (staBox1 != null && IsTextAllowed(staBox1.Text)) //if there is a text
                {
                    int x;
                    x = int.Parse(staBox1.Text); // convert to a date
                    station1 = x;
                    next(); // set the focus by some order
                }
                else { MessageBox.Show("the value is not courect"); }
            }
        }
        private void staBox2_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//if the user press on enter
            {
                if (staBox2 != null && IsTextAllowed(staBox2.Text)) //if there is a text
                {
                    int x;
                    x = int.Parse(staBox2.Text); // convert to a date
                    station2 = x;
                    next(); // set the focus by some order
                }
                else { MessageBox.Show("the value is not courect"); }
            }
        }

        private void distanceBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//if the user press on enter
            {
                if (distanceBox != null && IsTextAllowed(distanceBox.Text)) //if there is a text
                {
                    double x;
                    x = double.Parse(distanceBox.Text); // convert to a date
                    distance = x;
                    next(); // set the focus by some order
                }
                else { MessageBox.Show("the value is not courect"); }
            }
        }

        private void avergeBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)//if the user press on enter
            {
                if (avergeBox != null && IsTextAllowed(avergeBox.Text)) //if there is a text
                {
                    double x;
                    x = double.Parse(avergeBox.Text); // convert to a date
                    averageTime = TimeSpan.FromMinutes(x);
                    next(); // set the focus by some order
                }
                else { MessageBox.Show("the value is not courect"); }
            }
        }

        void next()
        {
            if (station1 == -1)
            {
                staBox1.Focus();
            }
            else if (station2 == -1)
            {
                staBox2.Focus();
            }
            else if (distance == 0)
            {
                distanceBox.Focus();
            }
            else if (averageTime == TimeSpan.Zero)
            {
                avergeBox.Focus();
            }
            else if (station1 != -1 && station2 != -1 && distance >= 0 && averageTime > TimeSpan.Zero)
                updateMe.Focus();
        }
    }
}
