using System;
using System.Globalization;
using System.Windows;

namespace DailyDungeon.Pages
{
    public partial class EditTaskWindow : Window
    {
        public string username { get; set; }

        private readonly string[] taskComplexity = { "Легко", "Середньо", "Складно" };
        private readonly string[] taskTags = { "Робота", "Навчання", "Здоров'я", "Хобі" };

        private tasks task = new tasks();

        public EditTaskWindow(string userName, tasks selectedTask)
        {
            InitializeComponent();
            username = userName;

            tagsComboBox.ItemsSource = taskTags;
            complexityComboBox.ItemsSource = taskComplexity;

            task = selectedTask;
            DataContext = task;

            DateTime deadline = DateTime.ParseExact(task.deadline_task, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            deadlineDatePicker.SelectedDate = deadline;
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(task.name_task))
            {
                MessageBox.Show("Не вдалося створити завдання! Обов'язково введіть його назву");
                return;
            }

            try
            {
                DataBaseModel.EditTask(task);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка при редагуванні завдання: {ex.Message}");
            }
            
            this.Hide();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
