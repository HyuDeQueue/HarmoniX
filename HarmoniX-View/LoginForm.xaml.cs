using HarmoniX_Repository.Models;
using HarmoniX_Service.Services;
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

namespace HarmoniX_View
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private readonly AccountService _accountService = new();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUpForm signUpForm = new SignUpForm();
            signUpForm.Show();
            this.Close();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            btnLogin.IsEnabled = false;
            btnLogin.Content = "";
            txtUser.IsEnabled = false;
            txtPass.IsEnabled = false;

            waveAnimation.Visibility = Visibility.Visible;

            string username = txtUser.Text;
            string password = txtPass.Password;

            Account account = new Account
            {
                Username = username,
                Password = password
            };

            Account loggedInAccount = null;
            try
            {
                loggedInAccount = await Task.Run(() => _accountService.LoginAsync(account));
            }
            finally
            {
                waveAnimation.Visibility = Visibility.Hidden;

                btnLogin.IsEnabled = true;
                btnLogin.Content = "LOGIN";
                txtUser.IsEnabled = true;
                txtPass.IsEnabled = true;
            }

            if (loggedInAccount != null)
            {
                MainWindow mainWindow = new MainWindow(loggedInAccount);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
