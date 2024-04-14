using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Globalization;
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
        public List<tasks> activeTasks = new List<tasks>();
        public List<tasks> overdueTasks = new List<tasks>();

        private readonly string[] taskSortCategory = { "Назва", "Опис", "Складність", "Дедлайн", "Тег" };

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
                var user = context.users.FirstOrDefault(u => u.login_user == username);
                if (user != null) moneyCount = user.money_count;
            }
            moneyCountText.Text = $"{moneyCount}";

            using (var context = new DailyDungeonEntities())
            {
                tasksList = context.tasks.Where(t => t.login_user == username).ToList();
            }
            foreach (var task in tasksList)
            {
                DateTime dateTime = DateTime.Now;
                DateTime deadline = DateTime.ParseExact(task.deadline_task, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                if (deadline.Date < dateTime.Date) overdueTasks.Add(task);
                else activeTasks.Add(task);
            }
            tasksDataGrid.ItemsSource = tasksList;
            activeTasksDataGrid.ItemsSource = activeTasks;
            overdueTasksDataGrid.ItemsSource = overdueTasks;
            
            sortComboBox.ItemsSource = taskSortCategory;
            sortComboBox.Loaded += ComboBox_Loaded;
            sortComboBox.SelectionChanged += ComboBox_SelectionChanged;

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

        public void Window_IsHitTestVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                using (var context = new DailyDungeonEntities())
                {
                    context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                    var user = context.users.FirstOrDefault(u => u.login_user == username);
                    if (user != null) moneyCount = user.money_count;
                    moneyCountText.Text = $"{moneyCount}";

                    activeTasks.Clear();
                    overdueTasks.Clear();
                    tasksList = context.tasks.Where(t => t.login_user == username).ToList();
                    foreach (var task in tasksList)
                    {
                        DateTime dateTime = DateTime.Now;
                        DateTime deadline = DateTime.ParseExact(task.deadline_task, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                        if (deadline.Date < dateTime.Date) overdueTasks.Add(task);
                        else activeTasks.Add(task);
                    }
                    tasksDataGrid.ItemsSource = tasksList;
                    activeTasksDataGrid.ItemsSource = activeTasks;
                    overdueTasksDataGrid.ItemsSource = overdueTasks;
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

                            activeTasks.Clear();
                            overdueTasks.Clear();
                            tasksList = context.tasks.Where(t => t.login_user == username).ToList();
                            foreach (var task in tasksList)
                            {
                                DateTime dateTime = DateTime.Now;
                                DateTime deadline = DateTime.ParseExact(task.deadline_task, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                if (deadline.Date < dateTime.Date) overdueTasks.Add(task);
                                else activeTasks.Add(task);
                            }
                            tasksDataGrid.ItemsSource = tasksList;
                            activeTasksDataGrid.ItemsSource = activeTasks;
                            overdueTasksDataGrid.ItemsSource = overdueTasks;

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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string selectedcategry = comboBox.SelectedItem as string;
            SortColumn(selectedcategry);
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                comboBox.Loaded -= ComboBox_Loaded;
            }
            string selectedcategry = comboBox.SelectedItem as string;
            SortColumn(selectedcategry);
        }

        private void SortColumn(string columnName)
        {
            var column = tasksDataGrid.Columns.SingleOrDefault(c => c.Header != null && c.Header.ToString() == columnName);

            if (column != null)
            {
                column.SortDirection = ListSortDirection.Ascending;
                tasksDataGrid.Items.SortDescriptions.Clear();
                tasksDataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, column.SortDirection.Value));
                activeTasksDataGrid.Items.SortDescriptions.Clear();
                activeTasksDataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, column.SortDirection.Value));
                overdueTasksDataGrid.Items.SortDescriptions.Clear();
                overdueTasksDataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, column.SortDirection.Value));
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            activeTasks.Clear();
            overdueTasks.Clear();

            using (var context = new DailyDungeonEntities())
            {
                tasksList = context.tasks.Where(t => t.login_user == username).ToList();
            }
            foreach (var task in tasksList)
            {
                DateTime dateTime = DateTime.Now;
                DateTime deadline = DateTime.ParseExact(task.deadline_task, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                if (deadline.Date < dateTime.Date) overdueTasks.Add(task);
                else activeTasks.Add(task);
            }

            tasksList = tasksList.Where(t => t.name_task.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
            activeTasks = activeTasks.Where(t => t.name_task.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
            overdueTasks = overdueTasks.Where(t => t.name_task.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            tasksDataGrid.ItemsSource = tasksList;
            activeTasksDataGrid.ItemsSource = activeTasks;
            overdueTasksDataGrid.ItemsSource = overdueTasks;
        }

        public void Window_Update()
        {
            activeTasks.Clear();
            overdueTasks.Clear();
            using (var context = new DailyDungeonEntities())
            {
                context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                tasksList = context.tasks.Where(t => t.login_user == username).ToList();
            }
            foreach (var task in tasksList)
            {
                DateTime dateTime = DateTime.Now;
                DateTime deadline = DateTime.ParseExact(task.deadline_task, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                if (deadline.Date < dateTime.Date) overdueTasks.Add(task);
                else activeTasks.Add(task);
            }
            tasksDataGrid.ItemsSource = tasksList;
            activeTasksDataGrid.ItemsSource = activeTasks;
            overdueTasksDataGrid.ItemsSource = overdueTasks;
        }
    }
}
