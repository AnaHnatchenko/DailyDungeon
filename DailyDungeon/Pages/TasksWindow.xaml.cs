using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
    public partial class TasksWindow : Window
    {
        public string username {  get; set; }
        public bool IsMaximized {  get; set; }

        public Color backgroundColor = Color.FromRgb(0x62, 0x3E, 0xD0);
        public ImageSource avatarImage = new BitmapImage(new Uri("D:/SUTE/ООП/Курсова/DailyDungeon/DailyDungeon/Resources/Images/Avatars/Avatar1.jpg", UriKind.Relative));

        public TasksWindow(string userName, bool isMaximized, Color Background, ImageSource Avatar)
        {
            InitializeComponent();
            IsMaximized = isMaximized;
            if (IsMaximized )
            {
                this.WindowState = WindowState.Maximized;
                IsMaximized = true;
            }

            this.Deactivated += Window_Deactivated;
            this.Activated += Window_Activated;

            username = "Anastasia";

            this.DataContext = this;

            string query = $"select * from {username}_tasks";
            tasksDataGrid.ItemsSource = DailyDungeonEntities.GetContext().Database.SqlQuery<tasks>(query).ToList();

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

        private void Habits_Click(object sender, RoutedEventArgs e)
        {
            var habitsWindow = new HabitsWindow(username, IsMaximized, backgroundColor, avatarImage);
            habitsWindow.Show();
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

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow();
            addTaskWindow.ShowDialog();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            tasks selectedTask = (tasks) tasksDataGrid.SelectedItem;

            string name = selectedTask.name_task;
            string description = selectedTask.description_task;
            string complexity = selectedTask.complexity_task;
            string tag = selectedTask.tag_task;
            DateTime deadline = DateTime.Parse(selectedTask.deadline_task);

            var editTaskWindow = new EditTaskWindow(name, description, complexity, tag, deadline);
            editTaskWindow.ShowDialog();
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
    }
}
