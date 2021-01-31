using System;
using System.Windows;
using BLApi;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bl = BLFactory.GetBL("1");
        private object content;
        public MainWindow()
        {
            InitializeComponent();
            content = Content; 
        }
        /// <summary>
        /// click butten to open manage window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGO_Click(object sender, RoutedEventArgs e)
        {
            if (userName.Text == string.Empty) 
            {
                userName.Focus();
            }
            else if (password.Password == string.Empty)
            {
                password.Focus();
            }
            else
            {
                if (bl.userCheck(userName.Text, password.Password, true))
                {
                    userName.Text = "";
                    password.Password = "";
                    newPassword.Password = "";
                    newUserName.Text = "";
                    confirmPassword.Password = "";
                    expander.IsExpanded = false;
                    ManageWindow winM = new ManageWindow(bl, this);
                    this.Content = winM.Content;

                }
                else
                {
                    MessageBox.Show("username or password incorrect", "error");
                    userName.Focus();
                }
            }
        }
        /// <summary>
        /// go back to the main frame window
        /// </summary>
        public void GoBackToStartPage()
        {
            Content = content;
        }
        /// <summary>
        /// sign up click butten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void singUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (confirmPassword.Password == "" || newPassword.Password == "" || newUserName.Text == "")
                    MessageBox.Show("a fild can't be empty");
                else if (confirmPassword.Password != newPassword.Password)
                    MessageBox.Show("the filds Password and Confirm Password are not matching");
                else
                {
                    bl.AddUser(newUserName.Text, newPassword.Password, true);
                    MessageBox.Show("the username successfully added");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error");
            }
        }
    }
}
