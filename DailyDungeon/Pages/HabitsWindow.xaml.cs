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
    public partial class HabitsWindow : Window
    {
        public string username { get; set; }
        public int moneyCount { get; set; }
        public bool IsMaximized { get; set; }

        public Color backgroundColor { get; set; } = Color.FromRgb(0x62, 0x3E, 0xD0);
        public ImageSource avatarImage { get; set; } = new BitmapImage(new Uri("D:/SUTE/ООП/Курсова/DailyDungeon/DailyDungeon/Resources/Images/Avatars/Avatar1.jpg", UriKind.Relative));

        private readonly string[] habitSortCategory = { "Назвою", "Описом", "Складністю", "Типом", "Тегом" };

        public HabitsWindow(string userName, bool isMaximized, Color Background, ImageSource Avatar)
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

            this.DataContext = this;

            string query = $"select * from {username}_habits";

            habitsDataGrid.ItemsSource = DailyDungeonEntities.GetContext().Database.SqlQuery<habits>(query).ToList();

            backgroundColor = Background;
            avatarImage = Avatar;
            background.Background = new SolidColorBrush(backgroundColor);
            avatar.Fill = new ImageBrush(avatarImage);

            sortComboBox.ItemsSource = habitSortCategory;
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

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            var inventoryWindow = new InventoryWindow(username, IsMaximized, backgroundColor, avatarImage);
            inventoryWindow.Show();
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
            Application.Current.Shutdown();
        }

        private void AddHabit_Click(object sender, RoutedEventArgs e)
        {
            var addHabitWindow = new AddHabitWindow(username);
            addHabitWindow.ShowDialog();
        }

        private void EditHabit_Click(object sender, RoutedEventArgs e)
        {
            habits selectedHabit = (habits)habitsDataGrid.SelectedItem;

            var editHabitWindow = new EditHabitWindow(username, selectedHabit);
            editHabitWindow.ShowDialog();
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

        private void Window_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                DailyDungeonEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                string query = $"select * from {username}_habits";
                habitsDataGrid.ItemsSource = DailyDungeonEntities.GetContext().Database.SqlQuery<habits>(query).ToList();
            }
        }

        private void DeleteHabit_Click(object sender, RoutedEventArgs e)
        {
            habits selectedHabit = (habits)habitsDataGrid.SelectedItem;
            if (MessageBox.Show("Ви дійсно бажаєте видалити це завдання?", "Увага", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    string deleteQuery = $"DELETE FROM {username}_habits WHERE id_habit = {selectedHabit.id_habit}";
                    DailyDungeonEntities.GetContext().Database.ExecuteSqlCommand(deleteQuery);
                    DailyDungeonEntities.GetContext().SaveChanges();
                    DailyDungeonEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    string reloadQuery = $"select * from {username}_habits";
                    habitsDataGrid.ItemsSource = DailyDungeonEntities.GetContext().Database.SqlQuery<habits>(reloadQuery).ToList();
                    MessageBox.Show($"Звичку видалено.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
