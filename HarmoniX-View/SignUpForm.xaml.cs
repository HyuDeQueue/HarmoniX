using HarmoniX_Repository.Models;
using HarmoniX_Service.Services;
using System.Windows;
using System.Windows.Input;

namespace HarmoniX_View
{
    public partial class SignUpForm : Window
    {
        private readonly AccountService _accountService = new();

        public SignUpForm()
        {
            InitializeComponent();
            txtName.KeyDown += TextBox_KeyDown;
            txtUser.KeyDown += TextBox_KeyDown;
            txtPass.KeyDown += TextBox_KeyDown;
            txtConfirmPass.KeyDown += TextBox_KeyDown;
        }

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

        private async void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            btnSignUp.IsEnabled = false;
            btnSignUp.Content = "";
            txtName.IsEnabled = false;
            txtUser.IsEnabled = false;
            txtPass.IsEnabled = false;
            txtConfirmPass.IsEnabled = false;

            waveAnimation.Visibility = Visibility.Visible;

            string name = txtName.Text.Trim();
            string username = txtUser.Text.Trim();
            string password = txtPass.Password;
            string confirmPassword = txtConfirmPass.Password;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.");
                waveAnimation.Visibility = Visibility.Hidden;
                btnSignUp.IsEnabled = true;
                btnSignUp.Content = "Sign Up";
                txtName.IsEnabled = true;
                txtUser.IsEnabled = true;
                txtPass.IsEnabled = true;
                txtConfirmPass.IsEnabled = true;
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                waveAnimation.Visibility = Visibility.Hidden;
                btnSignUp.IsEnabled = true;
                btnSignUp.Content = "Sign Up";
                txtName.IsEnabled = true;
                txtUser.IsEnabled = true;
                txtPass.IsEnabled = true;
                txtConfirmPass.IsEnabled = true;
                return;
            }

            var newAccount = new Account
            {
                DisplayName = name,
                Username = username,
                Password = password
            };

            try
            {
                var existingAccount = await Task.Run(() => _accountService.CheckAccountExistsAsync(newAccount));
                if (existingAccount != null)
                {
                    MessageBox.Show("Username already exists.");
                    waveAnimation.Visibility = Visibility.Hidden;
                    btnSignUp.IsEnabled = true;
                    btnSignUp.Content = "Sign Up";
                    txtName.IsEnabled = true;
                    txtUser.IsEnabled = true;
                    txtPass.IsEnabled = true;
                    txtConfirmPass.IsEnabled = true;
                    return;
                }
                await Task.Run(() => _accountService.RegisterAsync(newAccount));
                MessageBox.Show("Registration successful!");

                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                waveAnimation.Visibility = Visibility.Hidden;
                btnSignUp.IsEnabled = true;
                btnSignUp.Content = "Sign Up";
                txtName.IsEnabled = true;
                txtUser.IsEnabled = true;
                txtPass.IsEnabled = true;
                txtConfirmPass.IsEnabled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSignUp_Click(sender, new RoutedEventArgs());
            }
        }
    }
}
