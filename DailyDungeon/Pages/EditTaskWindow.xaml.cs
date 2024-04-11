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
        public string name { get; set; }
        public string description { get; set; }
        public string complexity { get; set; }
        public string tag { get; set; }
        public DateTime deadline { get; set; }

        private readonly string[] taskComplexity = { "Легко", "Середньо", "Складно" };
        private readonly string[] taskTags = { "Робота", "Навчання", "Здоров'я", "Хобі" };

        public EditTaskWindow(string Name, string Description, string Complexity, string Tag, DateTime Deadline)
        {
            InitializeComponent();
            InitializeComponent();
            this.Deactivated += ModalWindow_Deactivated;
            tagsComboBox.ItemsSource = taskTags;
            complexityComboBox.ItemsSource = taskComplexity;

            name = Name;
            description = Description;
            complexity = Complexity;
            tag = Tag;
            deadline = Deadline;

            txtName.Text = name;
            txtDescription.Text = description;
            complexityComboBox.SelectedItem = complexity;
            tagsComboBox.SelectedItem = tag;
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
            this.Hide();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void ModalWindow_Deactivated(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
