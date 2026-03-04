using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

            moneyCountText.Text = DataBaseModel.GetUserMoneyCount(username).ToString();
            background.Background = new SolidColorBrush(DataBaseModel.GetBackgroundColor(username));
            BitmapImage imageSource = new BitmapImage(new Uri(DataBaseModel.GetAvatarImage(username)));
            avatar.Fill = new ImageBrush(imageSource);
            
            avatarsList = DataBaseModel.GetAvatarsList(username);
            backgroundsList = DataBaseModel.GetBackgroundsList(username);

            for (int i = 0; i < backgroundsList.Count; i++) AddInventoryBackgroundObjects(inventoryBackgrounds, i);
            for (int i = 0; i < avatarsList.Count; i++) AddInventoryAvatarObjects(inventoryAvatars, i);
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
            Application.Current.Shutdown();
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
                    var inventoryObjectInfoWindow = new InventoryObjectInfoWindow(username, color);
                    inventoryObjectInfoWindow.ShowDialog();
                }
                else
                {
                    ImageSource image = ((ImageBrush)rectangle.Fill).ImageSource;
                    var inventoryObjectInfoWindow = new InventoryObjectInfoWindow(username, image);
                    inventoryObjectInfoWindow.ShowDialog();
                }
            }
        }

        private void AddInventoryBackgroundObjects(WrapPanel panel, int i)
        {
            Border border = new Border();
            border.Style = (Style)Application.Current.FindResource("objectBorder");
            border.MouseDown += Object_Click;

            Rectangle rectangle = new Rectangle();
            rectangle.Style = (Style)Application.Current.FindResource("objectImage");
            rectangle.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundsList[i].background_color));

            border.Child = rectangle;
            panel.Children.Add(border);
        }

        private void AddInventoryAvatarObjects(WrapPanel panel, int i)
        {
            Border border = new Border();
            border.Style = (Style)Application.Current.FindResource("objectBorder");
            border.MouseDown += Object_Click;

            Rectangle rectangle = new Rectangle();
            rectangle.Style = (Style)Application.Current.FindResource("objectImage");

            string dbPath = avatarsList[i].image_source;
            string fullPackUri = $"pack://application:,,,{dbPath}";
            ImageSource imageSource = new BitmapImage(new Uri(fullPackUri));
            rectangle.Fill = new ImageBrush(imageSource);

            border.Child = rectangle;
            panel.Children.Add(border);
        }

        private void Window_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                background.Background = new SolidColorBrush(DataBaseModel.GetBackgroundColor(username));
                BitmapImage imageSource = new BitmapImage(new Uri(DataBaseModel.GetAvatarImage(username)));
                avatar.Fill = new ImageBrush(imageSource);
            }
        }
    }
}
