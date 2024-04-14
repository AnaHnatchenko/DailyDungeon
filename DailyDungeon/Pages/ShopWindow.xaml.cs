using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
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
    public partial class ShopWindow : Window
    {
        public string username { get; set; }
        public int moneyCount { get; set; }
        public bool IsMaximized { get; set; }

        public List<Border> backgrounds = new List<Border>();
        public List<Border> avatars = new List<Border>();

        public ShopWindow(string userName, bool isMaximized)
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

            backgrounds.Clear();
            avatars.Clear();
            CreateShopBackgroundObjects(100);
            AddShopBackgroundObjects();
            CreateShopAvatarObjects(20);
            AddShopAvatarObjects();
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

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            var inventoryWindow = new InventoryWindow(username, IsMaximized);
            inventoryWindow.Show();
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

        private void CreateShopBackgroundObjects(int count)
        {
            Random random = new Random();

            for (int i = 0; i <= count; i++)
            {
                Border border = new Border();
                border.Style = (Style)Application.Current.FindResource("objectBorder");
                border.MouseDown += Object_Click;

                Rectangle rectangle = new Rectangle();
                rectangle.Style = (Style)Application.Current.FindResource("objectImage");
                rectangle.Fill = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));

                Border innerBorder = new Border();
                innerBorder.Style = (Style)Application.Current.FindResource("shopObjectInnerBorder");

                Image image = new Image();
                image.Style = (Style)Application.Current.FindResource("coinImage");

                TextBlock textBlock = new TextBlock();
                textBlock.Style = (Style)Application.Current.FindResource("priceTextBlock");

                StackPanel stackPanel = new StackPanel();
                stackPanel.Style = (Style)Application.Current.FindResource("shopObjectStackPanel");

                stackPanel.Children.Add(image);
                stackPanel.Children.Add(textBlock);
                innerBorder.Child = stackPanel;

                Grid.SetRow(rectangle, 0);
                Grid.SetRow(innerBorder, 1);

                Grid grid = new Grid();

                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

                grid.Children.Add(rectangle);
                grid.Children.Add(innerBorder);

                border.Child = grid;
                backgrounds.Add(border);
            }
        }

        private void CreateShopAvatarObjects(int count)
        {
            for (int i = 1; i < count; i++)
            {
                Border border = new Border();
                border.Style = (Style)Application.Current.FindResource("objectBorder");
                border.Name = $"Avatar{i}";
                border.MouseDown += Object_Click;

                Rectangle rectangle = new Rectangle();
                rectangle.Style = (Style)Application.Current.FindResource("shopObjectImageAvatar");


                string imagePath = $"D:/SUTE/ООП/Курсова/DailyDungeon/DailyDungeon/Resources/Images/Avatars/Avatar{i}.jpg";
                ImageSource imageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                rectangle.Fill = new ImageBrush(imageSource);

                Border innerBorder = new Border();
                innerBorder.Style = (Style)Application.Current.FindResource("shopObjectInnerBorder");

                Image image = new Image();
                image.Style = (Style)Application.Current.FindResource("coinImage");

                TextBlock textBlock = new TextBlock();
                textBlock.Style = (Style)Application.Current.FindResource("priceTextBlock");
                textBlock.Text = "100";

                StackPanel stackPanel = new StackPanel();
                stackPanel.Style = (Style)Application.Current.FindResource("shopObjectStackPanel");

                stackPanel.Children.Add(image);
                stackPanel.Children.Add(textBlock);
                innerBorder.Child = stackPanel;

                Grid.SetRow(rectangle, 0);
                Grid.SetRow(innerBorder, 1);

                Grid grid = new Grid();

                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

                grid.Children.Add(rectangle);
                grid.Children.Add(innerBorder);

                border.Child = grid;
                avatars.Add(border);
            }
        }

        private void AddShopBackgroundObjects()
        {
            shopBackgrounds.Children.Clear(); 
            foreach (var border in backgrounds)
            {
                shopBackgrounds.Children.Add(border);
            }
        }

        private void AddShopAvatarObjects()
        {
            List<string> inventory = new List<string>();
            using (var context = new DailyDungeonEntities())
            {
                inventory = context.avatars.Where(i => i.login_user == username).Select(i => i.image_source).ToList();
            }
            for (int i = 0; i < inventory.Count; i++)
            {
                string temp = System.IO.Path.GetFileNameWithoutExtension(inventory[i]);
                inventory[i] = temp;
            }
            avatars.RemoveAll(border => inventory.Contains(border.Name));

            shopAvatars.Children.Clear();
            foreach (var border in avatars)
            {
                shopAvatars.Children.Add(border);
            }
        }

        private void Object_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Child is Grid grid && grid.Children.Count > 0 && grid.Children[0] is Rectangle rectangle)
            { 
                if (rectangle.Fill is SolidColorBrush solidColorBrush)
                {
                    Color color = solidColorBrush.Color;
                    var shopObjectInfoWindow = new ShopObjectInfoWindow(border, username, color);
                    shopObjectInfoWindow.BuyObject += BuyObject;
                    shopObjectInfoWindow.ShowDialog();
                }
                else
                {
                    var shopObjectInfoWindow = new ShopObjectInfoWindow(border, username, ((ImageBrush)rectangle.Fill).ImageSource);
                    shopObjectInfoWindow.BuyObject += BuyObject;
                    shopObjectInfoWindow.ShowDialog();
                }
            }
        }

        private void BuyObject(object sender, Border Object)
        {
            if (Object.Child is Grid grid && grid.Children.Count > 0 && grid.Children[0] is Rectangle rectangle)
            {
                if (rectangle.Fill is SolidColorBrush solidColorBrush)
                {
                    backgrounds.RemoveAll(border => border == Object);
                    AddShopBackgroundObjects();
                }
                else
                {
                    AddShopAvatarObjects();
                }
            }

        }

        private void Window_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                using (var context = new DailyDungeonEntities())
                {
                    var user = context.users.FirstOrDefault(u => u.login_user == username);
                    if (user != null) moneyCount = user.money_count;
                }
                moneyCountText.Text = $"{moneyCount}";
            }
        }
    }
}
