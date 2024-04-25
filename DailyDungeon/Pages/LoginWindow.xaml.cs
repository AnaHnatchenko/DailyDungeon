using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DailyDungeon.Pages
{
    public partial class LoginWindow : Window
    {
        public string login { get; set; }
        public string password { get; set; }
        public bool IsMaximized { get; set; } = false;

        public LoginWindow()
        {
            InitializeComponent();
            IsMaximized = false;
        }

        public LoginWindow(bool isMaximized)
        {
            InitializeComponent();
            IsMaximized = isMaximized;
            if (IsMaximized)
            {
                this.WindowState = WindowState.Maximized;
                IsMaximized = true;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 800;
                    this.Height = 500;
                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized = true;
                }
            }
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0) textPassword.Visibility = Visibility.Collapsed;
            else textPassword.Visibility = Visibility.Visible;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow(IsMaximized);
            registerWindow.Show();
            this.Close();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            login = txtLogin.Text;
            password = txtPassword.Password;

            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(login) && string.IsNullOrEmpty(password)) errors.AppendLine("Будь ласка введіть ваш логін та пароль!");
            else if (string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password)) errors.AppendLine("Будь ласка введіть ваш логін!");
            else if (!string.IsNullOrEmpty(login) && string.IsNullOrEmpty(password)) errors.AppendLine("Будь ласка введіть ваш пароль!");
            else if (!DataBaseModel.IsUserExists(login, password)) errors.AppendLine("Неправильний логін або пароль! Такого користувача не існує. Спробуйте ще раз або зареєструйте новий акаунт!");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            else
            {
                var tasksWindow = new TasksWindow(login, IsMaximized);
                tasksWindow.Show();
                this.Close();
            }
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }
    }
}
