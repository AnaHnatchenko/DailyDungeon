using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

            Window_Update();

            background.Background = new SolidColorBrush(DataBaseModel.GetBackgroundColor(username));
            BitmapImage imageSource = new BitmapImage(new Uri(DataBaseModel.GetAvatarImage(username)));
            avatar.Fill = new ImageBrush(imageSource);

            sortComboBox.ItemsSource = taskSortCategory;
            sortComboBox.Loaded += ComboBox_Loaded;
            sortComboBox.SelectionChanged += ComboBox_SelectionChanged;
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
            if (Visibility == Visibility.Visible) Window_Update();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseModel.DeleteTask(username, tasksDataGrid.SelectedItem as tasks);
                Window_Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка при видаленні завдання: {ex.Message}");
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

            tasksList = DataBaseModel.GetTasksForUser(username);

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

            DataBaseModel.ReloadData();
            moneyCountText.Text = DataBaseModel.GetUserMoneyCount(username).ToString();
            tasksList = DataBaseModel.GetTasksForUser(username);

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
