using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
        public int moneyCount { get; set; }
        public bool IsMaximized {  get; set; }
        public List<tasks> tasksList = new List<tasks>();

        private readonly string[] taskSortCategory = { "Назвою", "Описом", "Складністю", "Дедлайном", "Тегом" };

        public TasksWindow(string userName, bool isMaximized)
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
            this.IsHitTestVisibleChanged += Window_IsHitTestVisibleChanged;
            this.DataContext = this;

            username = userName;
            userTextBlock.Text = username;
            
            using (var context = new DailyDungeonEntities())
            {
                tasksList = context.tasks.Where(t => t.login_user == username).ToList();
                tasksDataGrid.ItemsSource = tasksList;

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
            
            sortComboBox.ItemsSource = taskSortCategory;

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

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow(username);
            addTaskWindow.ShowDialog();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            tasks selectedTask = (tasks) tasksDataGrid.SelectedItem;
            var editTaskWindow = new EditTaskWindow(username, selectedTask);
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

        private void Window_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                using (var context = new DailyDungeonEntities())
                {
                    context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                    tasksList = context.tasks.Where(t => t.login_user == username).ToList();
                    tasksDataGrid.ItemsSource = tasksList;
                }
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            tasks selectedTask = (tasks)tasksDataGrid.SelectedItem;
            if (MessageBox.Show("Ви дійсно бажаєте видалити це завдання?", "Увага", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        var taskToDelete = context.tasks.FirstOrDefault(t => t.id_task == selectedTask.id_task && t.login_user == username);
                        if (taskToDelete != null)
                        {
                            context.tasks.Remove(taskToDelete);
                            context.SaveChanges();

                            tasksList = context.tasks.Where(t => t.login_user == username).ToList();
                            tasksDataGrid.ItemsSource = tasksList;

                            MessageBox.Show("Завдання видалено.");
                        }
                        else
                        {
                            MessageBox.Show("Завдання не знайдено або вже було видалено.");
                        }
                    }    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при видаленні завдання: {ex.Message}");
                }
            }
        }
    }
}
