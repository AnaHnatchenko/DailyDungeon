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

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            task.name_task = task.name_task.Trim();
            task.description_task = task.description_task.Trim();
            if (string.IsNullOrWhiteSpace(task.tag_task)) task.tag_task = "";
            task.deadline_task = deadlineDatePicker.SelectedDate?.ToString("dd.MM.yyyy");
            string query = $"update {username}_tasks set name_task = '{task.name_task}', description_task = '{task.description_task}', complexity_task = '{task.complexity_task}', " +
                $"tag_task = '{task.tag_task}', deadline_task = '{task.deadline_task}' where id_task = '{task.id_task}'";
            try
            {
                DailyDungeonEntities.GetContext().Database.ExecuteSqlCommand(query);
                DailyDungeonEntities.GetContext().SaveChanges();
                MessageBox.Show($"Завдання успішно відредаговано!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            this.Hide();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
