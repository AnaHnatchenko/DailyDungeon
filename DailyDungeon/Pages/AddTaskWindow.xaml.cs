using System;
using System.Windows;

namespace DailyDungeon.Pages
{
    public partial class AddTaskWindow : Window
    {
        public string username { get; set; }
        private readonly string[] taskComplexity = {"Легко", "Середньо", "Складно"};
        private readonly string[] taskTags = { "Робота", "Навчання", "Здоров'я", "Хобі"};
        private tasks task = new tasks();

        public AddTaskWindow(string userName)
        {
            InitializeComponent();
            username = userName;

            tagsComboBox.ItemsSource = taskTags;
            complexityComboBox.ItemsSource = taskComplexity;

            DataContext = task;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(task.name_task))
            {
                MessageBox.Show("Не вдалося створити завдання! Обов'язково введіть його назву");
                return;
            }

            try
            {
                task.login_user = username;
                task.name_task = task.name_task.Trim();
                task.description_task = string.IsNullOrWhiteSpace(task.description_task) ? string.Empty : task.description_task.Trim();
                task.tag_task = string.IsNullOrWhiteSpace(task.tag_task) ? string.Empty : task.tag_task.Trim();
                task.deadline_task = deadlineDatePicker.SelectedDate?.ToString("dd.MM.yyyy");
                task.is_done = false;

                DataBaseModel.CreateTask(task);
                MessageBox.Show("Завдання успішно створено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка при створенні завдання: {ex.Message}");
            }

            this.Hide();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
