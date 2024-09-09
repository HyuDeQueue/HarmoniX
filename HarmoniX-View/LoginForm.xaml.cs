using HarmoniX_Repository.Models;
using HarmoniX_Service.Services;
using System.Windows;
using System.Windows.Input;

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
            txtUser.KeyDown += TextBox_KeyDown;
            txtPass.KeyDown += TextBox_KeyDown;
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
            Close();
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
                Close();
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


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
