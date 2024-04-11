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
    public partial class ShopWindow : Window
    {
        public string username { get; set; }
        public bool IsMaximized { get; set; }
        public ShopWindow(string userName, bool isMaximized)
        {
            InitializeComponent();
            AddShopBackgroundObjects(shopBackgrounds, 100);
            AddShopAvatarObjects(shopAvatars, 16);

            IsMaximized = isMaximized;
            if (IsMaximized)
            {
                this.WindowState = WindowState.Maximized;
                IsMaximized = true;
            }

            this.Deactivated += Window_Deactivated;
            this.Activated += Window_Activated;

            username = "Anastasia";
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
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.IsHitTestVisible = true;
        }

        private void AddShopBackgroundObjects(WrapPanel panel, int count)
        {
            Random random = new Random();

            for (int i = 0; i <= count; i++)
            {
                Border border = new Border();
                border.Style = (Style)Application.Current.FindResource("objectBorder");

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
                panel.Children.Add(border);
            }
        }

        private void AddShopAvatarObjects(WrapPanel panel, int count)
        {
            for (int i = 3; i < count; i++)
            {
                Border border = new Border();
                border.Style = (Style)Application.Current.FindResource("objectBorder");

                Rectangle rectangle = new Rectangle();
                rectangle.Style = (Style)Application.Current.FindResource("shopObjectImageAvatar");


                string imagePath = $"D:/SUTE/ООП/Курсова/DailyDungeon/DailyDungeon/Resources/Images/Avatars/Avatar{i}.jpg";
                ImageSource imageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                rectangle.Fill = new ImageBrush(imageSource); ;

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
                panel.Children.Add(border);
            }
        }
    }
}
