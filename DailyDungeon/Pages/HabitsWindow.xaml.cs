using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DailyDungeon.Pages
{
    public partial class HabitsWindow : Window
    {
        public string username { get; set; }
        public int moneyCount { get; set; }
        public bool IsMaximized { get; set; }

        public List<habits> habitsList = new List<habits>();
        public List<habits> positiveHabits = new List<habits>();
        public List<habits> negativeHabits = new List<habits>();
        public List<habits> neutralHabits = new List<habits>();

        private readonly string[] habitSortCategory = { "Назва", "Опис", "Складність", "Тип", "Тег" };

        public HabitsWindow(string userName, bool isMaximized)
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
            this.DataContext = this;

            username = userName;
            userTextBlock.Text = username;

            Window_Update();

            background.Background = new SolidColorBrush(DataBaseModel.GetBackgroundColor(username));
            BitmapImage imageSource = new BitmapImage(new Uri(DataBaseModel.GetAvatarImage(username)));
            avatar.Fill = new ImageBrush(imageSource);

            sortComboBox.ItemsSource = habitSortCategory;
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

        private void Tasks_Click(object sender, RoutedEventArgs e)
        {
            var tasksWindow = new TasksWindow(username, IsMaximized);
            tasksWindow.Show();
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
            if (Visibility == Visibility.Visible) Window_Update();
        }

        private void DeleteHabit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataBaseModel.DeleteHabit(username, habitsDataGrid.SelectedItem as habits);
                Window_Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка при видаленні звички: {ex.Message}");
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
            var column = habitsDataGrid.Columns.SingleOrDefault(c => c.Header != null && c.Header.ToString() == columnName);

            if (column != null)
            {
                column.SortDirection = ListSortDirection.Ascending;
                habitsDataGrid.Items.SortDescriptions.Clear();
                habitsDataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, column.SortDirection.Value));
                positiveHabitsDataGrid.Items.SortDescriptions.Clear();
                positiveHabitsDataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, column.SortDirection.Value));
                negativeHabitsDataGrid.Items.SortDescriptions.Clear();
                negativeHabitsDataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, column.SortDirection.Value));
                neutralHabitsDataGrid.Items.SortDescriptions.Clear();
                neutralHabitsDataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, column.SortDirection.Value));
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            positiveHabits.Clear();
            negativeHabits.Clear();
            neutralHabits.Clear();

            habitsList = DataBaseModel.GetHabitsForUser(username);

            foreach (var habit in habitsList)
            {
                if (habit.type_habit == "Позитивна") positiveHabits.Add(habit);
                else if (habit.type_habit == "Негативна") negativeHabits.Add(habit);
                else neutralHabits.Add(habit);
            }

            habitsList = habitsList.Where(t => t.name_habit.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
            positiveHabits = positiveHabits.Where(t => t.name_habit.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
            negativeHabits = negativeHabits.Where(t => t.name_habit.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
            neutralHabits = neutralHabits.Where(t => t.name_habit.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            habitsDataGrid.ItemsSource = habitsList;
            positiveHabitsDataGrid.ItemsSource = positiveHabits;
            negativeHabitsDataGrid.ItemsSource = negativeHabits;
            neutralHabitsDataGrid.ItemsSource = neutralHabits;
        }

        public void Window_Update()
        {
            positiveHabits.Clear();
            negativeHabits.Clear();
            neutralHabits.Clear();

            DataBaseModel.ReloadData();
            moneyCountText.Text = DataBaseModel.GetUserMoneyCount(username).ToString();
            habitsList = DataBaseModel.GetHabitsForUser(username);

            foreach (var habit in habitsList)
            {
                if (habit.type_habit == "Позитивна") positiveHabits.Add(habit);
                else if (habit.type_habit == "Негативна") negativeHabits.Add(habit);
                else neutralHabits.Add(habit);
            }

            habitsDataGrid.ItemsSource = habitsList;
            positiveHabitsDataGrid.ItemsSource = positiveHabits;
            negativeHabitsDataGrid.ItemsSource = negativeHabits;
            neutralHabitsDataGrid.ItemsSource = neutralHabits;
        }
    }
}
