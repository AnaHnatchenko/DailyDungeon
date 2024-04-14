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

namespace DailyDungeon.Pages
{
    public partial class RegisterWindow : Window
    {
        public string login { get; set; }
        public string password { get; set; }
        public bool IsMaximized { get; set; }

        public RegisterWindow(bool isMaximized)
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
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
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
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow(IsMaximized);
            loginWindow.Show();
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            login = txtLogin.Text;
            password = txtPassword.Password;

            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(login) && string.IsNullOrEmpty(password)) errors.AppendLine("Будь ласка введіть логін та пароль!");
            else if (string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password)) errors.AppendLine("Будь ласка введіть логін!");
            else if (!string.IsNullOrEmpty(login) && string.IsNullOrEmpty(password)) errors.AppendLine("Будь ласка введіть пароль!");
            else if (IsLoginExists(login) && !IsUserExists(login, password)) errors.AppendLine("Користувач з таким логіном вже існує. Будь ласка оберіть інше ім'я!");
            else if (IsUserExists(login, password)) errors.AppendLine("Такий користувач вже існує. Будь ласка увійдіть до свого акаунту!");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            else
            {
                users newUser = new users()
                {
                    login_user = login,
                    password_user = password,
                    money_count = 100
                };

                avatars avatar = new avatars()
                {
                    login_user = login,
                    image_source = "D:/SUTE/ООП/Курсова/DailyDungeon/DailyDungeon/Resources/Images/Avatars/Avatar1.jpg",
                    is_used = true
                };

                backgrounds background = new backgrounds()
                {
                    login_user = login,
                    background_color = "#623ed0",
                    is_used = true
                };

                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        context.users.Add(newUser);
                        context.avatars.Add(avatar);
                        context.backgrounds.Add(background);
                        context.SaveChanges();

                        MessageBox.Show("Акаунт успішно створено!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при створенні акаунту: {ex.Message}");
                }

                var loginWindow = new LoginWindow(IsMaximized);
                loginWindow.Show();
                this.Close();
            }
        }

        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }

        public bool IsUserExists(string login, string password)
        {
            using (var context = new DailyDungeonEntities())
            {
                bool userExists = context.users.Any(u => u.login_user == login && u.password_user == password);
                return userExists;
            }
        }

        public bool IsLoginExists(string login)
        {
            using (var context = new DailyDungeonEntities())
            {
                bool userExists = context.users.Any(u => u.login_user == login);
                return userExists;
            }
        }
    }
}
