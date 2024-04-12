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
    public partial class AddHabitWindow : Window
    {
        public string username { get; set; }
        private readonly string[] habitComplexity = { "Легко", "Середньо", "Складно" };
        private readonly string[] habitType = { "Позитивна", "Нейтральна", "Негативна" };
        private readonly string[] habitTags = { "Робота", "Навчання", "Здоров'я", "Хобі" };
        private habits habit = new habits();

        public AddHabitWindow(string userName)
        {
            InitializeComponent();
            username = userName;

            complexityComboBox.ItemsSource = habitComplexity;
            typeComboBox.ItemsSource = habitType;
            tagsComboBox.ItemsSource = habitTags;

            DataContext = habit;
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

        private void AddHabit_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(habit.name_habit)) errors.AppendLine("Не вдалося створити завдання! Обов'язково введіть його назву");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            else
            {
                habit.name_habit = habit.name_habit.Trim();
                habit.description_habit = habit.description_habit.Trim();
                if (string.IsNullOrWhiteSpace(habit.tag_habit)) habit.tag_habit = "";
                string query = $"insert into {username}_habits (name_habit, description_habit, complexity_habit, tag_habit, type_habit, is_done) " +
                    $"values ('{habit.name_habit}', '{habit.description_habit}', '{habit.complexity_habit}', '{habit.tag_habit}', '{habit.type_habit}', 0)";
                try
                {
                    DailyDungeonEntities.GetContext().Database.ExecuteSqlCommand(query);
                    DailyDungeonEntities.GetContext().SaveChanges();
                    MessageBox.Show($"Звичку успішно створено!");
                }
                catch (Exception ex)
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
