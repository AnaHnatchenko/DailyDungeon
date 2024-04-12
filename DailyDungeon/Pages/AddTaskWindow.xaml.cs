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

        public class SelectedItemExistsConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value != null;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class SelectedDateExistsConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is DateTime selectedDate)
                {
                    return selectedDate != DateTime.MinValue;
                }
                return false;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(task.name_task)) errors.AppendLine("Не вдалося створити завдання! Обов'язково введіть його назву");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            else
            {
                task.name_task = task.name_task.Trim();
                task.description_task = task.description_task.Trim();
                if (string.IsNullOrWhiteSpace(task.tag_task)) task.tag_task="";
                task.deadline_task = deadlineDatePicker.SelectedDate?.ToString("dd.MM.yyyy");
                string query = $"insert into {username}_tasks (name_task, description_task, complexity_task, tag_task, deadline_task, is_done) " +
                    $"values ('{task.name_task}', '{task.description_task}', '{task.complexity_task}', '{task.tag_task}', '{task.deadline_task}', 0)";
                try
                {
                    DailyDungeonEntities.GetContext().Database.ExecuteSqlCommand(query);
                    DailyDungeonEntities.GetContext().SaveChanges();
                    MessageBox.Show($"Завдання успішно створено!");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
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
