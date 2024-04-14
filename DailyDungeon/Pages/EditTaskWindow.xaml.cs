using System;
using System.Collections.Generic;
using System.Globalization;
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
            else
            {
                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        var taskToUpdate = context.tasks.FirstOrDefault(t => t.id_task == task.id_task);
                        if (taskToUpdate != null)
                        {
                            taskToUpdate.login_user = username;
                            taskToUpdate.name_task = task.name_task.Trim();
                            if (string.IsNullOrWhiteSpace(task.description_task)) taskToUpdate.description_task = String.Empty;
                            else taskToUpdate.description_task = task.description_task.Trim();
                            taskToUpdate.complexity_task = task.complexity_task;
                            if (string.IsNullOrWhiteSpace(task.tag_task)) taskToUpdate.tag_task = String.Empty;
                            else taskToUpdate.tag_task = task.tag_task.Trim();
                            taskToUpdate.deadline_task = deadlineDatePicker.SelectedDate?.ToString("dd.MM.yyyy");
                            taskToUpdate.is_done = false;

                            context.SaveChanges();
                            MessageBox.Show("Завдання успішно відредаговано!");
                        }
                        else
                        {
                            MessageBox.Show("Завдання не знайдено.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при редагуванні завдання: {ex.Message}");
                }
            }
            
            this.Hide();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
