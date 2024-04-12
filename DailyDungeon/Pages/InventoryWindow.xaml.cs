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
using static System.Net.Mime.MediaTypeNames;

namespace DailyDungeon.Pages
{
    public partial class InventoryWindow : Window
    {
        public string username { get; set; }
        public int moneyCount { get; set; }
        public bool IsMaximized { get; set; }

        public Color backgroundColor { get; set; } = Color.FromRgb(0x62, 0x3E, 0xD0);
        public ImageSource avatarImage { get; set; } = new BitmapImage(new Uri("D:/SUTE/ООП/Курсова/DailyDungeon/DailyDungeon/Resources/Images/Avatars/Avatar1.jpg", UriKind.Relative));

        public InventoryWindow(string userName, bool isMaximized, Color Background, ImageSource Avatar)
        {
            InitializeComponent();

            IsMaximized = isMaximized;
            if (IsMaximized)
            {
                this.WindowState = WindowState.Maximized;
                IsMaximized = true;
            }

            this.Deactivated += Window_Deactivated;
            this.Activated += Window_Activated;

            username = "Anastasia";
            userTextBlock.Text = username;
            using (var context = new DailyDungeonEntities())
            {
                var user = context.users.FirstOrDefault(u => u.login_user == username);
                if (user != null)
                {
                    moneyCount = user.money_count;
                }
                else
                {
                    moneyCount = 0;
                }
            }
            moneyCountText.Text = $"{moneyCount}";

            backgroundColor = Background;
            avatarImage = Avatar;
            background.Background = new SolidColorBrush(backgroundColor);
            avatar.Fill = new ImageBrush(avatarImage);
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
                    this.Width = 1080;
                    this.Height = 720;
                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized = true;
                }
            }
        }

        private void Tasks_Click(object sender, RoutedEventArgs e)
        {
            var tasksWindow = new TasksWindow(username, IsMaximized, backgroundColor, avatarImage);
            tasksWindow.Show();
            this.Close();
        }

        private void Habits_Click(object sender, RoutedEventArgs e)
        {
            var habitsWindow = new HabitsWindow(username, IsMaximized, backgroundColor, avatarImage);
            habitsWindow.Show();
            this.Close();
        }
        private void Shop_Click(object sender, RoutedEventArgs e)
        {
            var shopWindow = new ShopWindow(username, IsMaximized, backgroundColor, avatarImage);
            shopWindow.Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow(IsMaximized);
            loginWindow.Show();
            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.IsHitTestVisible = false;
            Overlay.Visibility = Visibility.Visible;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.IsHitTestVisible = true;
            Overlay.Visibility = Visibility.Collapsed;
        }

        private void Object_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Child is Rectangle rectangle)
            {
                if (rectangle.Fill is SolidColorBrush solidColorBrush)
                {
                    Color color = solidColorBrush.Color;
                    Color backgroundColor = ((SolidColorBrush)background.Background).Color;
                    var inventoryObjectInfoWindow = new InventoryObjectInfoWindow(username, backgroundColor, color);
                    inventoryObjectInfoWindow.UseBackgroundObject += ChangeBackground;
                    inventoryObjectInfoWindow.ShowDialog();
                }
                else
                {
                    ImageSource image = ((ImageBrush)rectangle.Fill).ImageSource;
                    ImageSource avatarImage = ((ImageBrush)avatar.Fill).ImageSource;
                    var inventoryObjectInfoWindow = new InventoryObjectInfoWindow(username, avatarImage, image);
                    inventoryObjectInfoWindow.UseAvatarObject += ChangeAvatar;
                    inventoryObjectInfoWindow.ShowDialog();
                }
            }
        }

        private void ChangeBackground(object sender, Color color)
        {
            backgroundColor = color;
            this.background.Background = new SolidColorBrush(backgroundColor);
            this.background.Opacity = 80;
        }

        private void ChangeAvatar(object sender, ImageSource image)
        {
            avatarImage = image;
            this.avatar.Fill = new ImageBrush(avatarImage);
        }
    }
}
