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
using System.Xml.Linq;

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
            else
            {
                task.login_user = username;
                task.name_task = task.name_task.Trim();
                if (string.IsNullOrWhiteSpace(task.description_task)) task.description_task = String.Empty;
                else task.description_task = task.description_task.Trim();
                if (string.IsNullOrWhiteSpace(task.tag_task)) task.tag_task = String.Empty;
                else task.tag_task = task.tag_task.Trim();
                task.deadline_task = deadlineDatePicker.SelectedDate?.ToString("dd.MM.yyyy");
                task.is_done = false;

                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        context.tasks.Add(task);
                        context.SaveChanges();

                        MessageBox.Show("Завдання успішно створено!");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при створенні завдання: {ex.Message}");
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
