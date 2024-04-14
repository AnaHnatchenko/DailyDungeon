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

        public List<avatars> avatarsList = new List<avatars>();
        public List<backgrounds> backgroundsList = new List<backgrounds>();

        public InventoryWindow(string userName, bool isMaximized)
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
            this.IsHitTestVisibleChanged += Window_IsHitTestVisibleChanged;

            username = userName;
            userTextBlock.Text = username;
            using (var context = new DailyDungeonEntities())
            {
                var user = context.users.FirstOrDefault(u => u.login_user == username);
                if (user != null) moneyCount = user.money_count;
            }
            moneyCountText.Text = $"{moneyCount}";

            Color backgroundColor;
            using (var context = new DailyDungeonEntities())
            {
                backgroundColor = (Color)ColorConverter.ConvertFromString(context.backgrounds.Where(b => b.login_user == username && b.is_used).Select(b => b.background_color).FirstOrDefault());
            }
            background.Background = new SolidColorBrush(backgroundColor);

            string avatarImage;
            using (var context = new DailyDungeonEntities())
            {
                avatarImage = context.avatars.Where(a => a.login_user == username && a.is_used).Select(a => a.image_source).FirstOrDefault();
            }
            BitmapImage imageSource = new BitmapImage(new Uri(avatarImage));
            avatar.Fill = new ImageBrush(imageSource);

            using (var context = new DailyDungeonEntities())
            {
                avatarsList = context.avatars.Where(a => a.login_user == username).ToList();
                backgroundsList = context.backgrounds.Where(a => a.login_user == username).ToList();
            }

            for (int i = 0; i < backgroundsList.Count; i++) AddInventoryBackgroundObjects(inventoryBackgrounds, i);
            for (int i = 0; i < avatarsList.Count; i++) AddInventoryAvatarObjects(inventoryAvatars, i);
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
            var tasksWindow = new TasksWindow(username, IsMaximized);
            tasksWindow.Show();
            this.Close();
        }

        private void Habits_Click(object sender, RoutedEventArgs e)
        {
            var habitsWindow = new HabitsWindow(username, IsMaximized);
            habitsWindow.Show();
            this.Close();
        }
        private void Shop_Click(object sender, RoutedEventArgs e)
        {
            var shopWindow = new ShopWindow(username, IsMaximized);
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
                    var inventoryObjectInfoWindow = new InventoryObjectInfoWindow(username, color);
                    inventoryObjectInfoWindow.ShowDialog();
                }
                else
                {
                    ImageSource image = ((ImageBrush)rectangle.Fill).ImageSource;
                    ImageSource avatarImage = ((ImageBrush)avatar.Fill).ImageSource;
                    var inventoryObjectInfoWindow = new InventoryObjectInfoWindow(username, image);
                    inventoryObjectInfoWindow.ShowDialog();
                }
            }
        }

        private void AddInventoryBackgroundObjects(WrapPanel panel, int i)
        {
            Border border = new Border();
            border.Style = (Style)System.Windows.Application.Current.FindResource("objectBorder");
            border.MouseDown += Object_Click;

            Rectangle rectangle = new Rectangle();
            rectangle.Style = (Style)System.Windows.Application.Current.FindResource("objectImage");
            rectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundsList[i].background_color));

            border.Child = rectangle;
            panel.Children.Add(border);
        }

        private void AddInventoryAvatarObjects(WrapPanel panel, int i)
        {
            Border border = new Border();
            border.Style = (Style)System.Windows.Application.Current.FindResource("objectBorder");
            border.MouseDown += Object_Click;

            Rectangle rectangle = new Rectangle();
            rectangle.Style = (Style)System.Windows.Application.Current.FindResource("objectImage");

            string imagePath = $"{avatarsList[i].image_source}";
            ImageSource imageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            rectangle.Fill = new ImageBrush(imageSource);

            border.Child = rectangle;
            panel.Children.Add(border);
        }

        private void Window_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Color backgroundColor;
                using (var context = new DailyDungeonEntities())
                {
                    backgroundColor = (Color)ColorConverter.ConvertFromString(context.backgrounds.Where(b => b.login_user == username && b.is_used).Select(b => b.background_color).FirstOrDefault());
                }
                background.Background = new SolidColorBrush(backgroundColor);

                string avatarImage;
                using (var context = new DailyDungeonEntities())
                {
                    avatarImage = context.avatars.Where(a => a.login_user == username && a.is_used).Select(a => a.image_source).FirstOrDefault();
                }
                BitmapImage imageSource = new BitmapImage(new Uri(avatarImage));
                avatar.Fill = new ImageBrush(imageSource);
            }
        }
    }
}
