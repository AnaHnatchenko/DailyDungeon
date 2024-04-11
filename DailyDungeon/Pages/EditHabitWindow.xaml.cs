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
    public partial class EditHabitWindow : Window
    {
        public string name { get; set; }
        public string description { get; set; }
        public string complexity { get; set; }
        public string type { get; set; }
        public string tag { get; set; }

        private readonly string[] habitComplexity = { "Легко", "Середньо", "Складно" };
        private readonly string[] habitType = { "Позитивна", "Нейтральна", "Негативна" };
        private readonly string[] habitTags = { "Робота", "Навчання", "Здоров'я", "Хобі" };

        public EditHabitWindow(string Name, string Description, string Complexity, string Type, string Tag)
        {
            InitializeComponent();
            this.Deactivated += ModalWindow_Deactivated;
            complexityComboBox.ItemsSource = habitComplexity;
            typeComboBox.ItemsSource = habitType;
            tagsComboBox.ItemsSource = habitTags;

            name = Name;
            description = Description;
            complexity = Complexity;
            type = Type;
            tag = Tag;

            txtName.Text = name;
            txtDescription.Text = description;
            complexityComboBox.SelectedItem = complexity;
            typeComboBox.SelectedItem = type;
            tagsComboBox.SelectedItem = tag;
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

        private void EditHabit_Click(object sender, RoutedEventArgs e)
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
