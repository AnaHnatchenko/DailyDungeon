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
        public string username { get; set; }

        private readonly string[] habitComplexity = { "Легко", "Середньо", "Складно" };
        private readonly string[] habitType = { "Позитивна", "Нейтральна", "Негативна" };
        private readonly string[] habitTags = { "Робота", "Навчання", "Здоров'я", "Хобі" };

        private habits habit = new habits();

        public EditHabitWindow(string userName, habits selectedHabit)
        {
            InitializeComponent();
            username = userName;

            complexityComboBox.ItemsSource = habitComplexity;
            typeComboBox.ItemsSource = habitType;
            tagsComboBox.ItemsSource = habitTags;

            habit = selectedHabit;
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

        private void EditHabit_Click(object sender, RoutedEventArgs e)
        {
            habit.name_habit = habit.name_habit.Trim();
            habit.description_habit = habit.description_habit.Trim();
            if (string.IsNullOrWhiteSpace(habit.tag_habit)) habit.tag_habit = "";
            string query = $"update {username}_habits set name_habit = '{habit.name_habit}', description_habit = '{habit.description_habit}', complexity_habit = '{habit.complexity_habit}', " +
                $"tag_habit = '{habit.tag_habit}', type_habit = '{habit.type_habit}' where id_habit = '{habit.id_habit}'";
            try
            {
                DailyDungeonEntities.GetContext().Database.ExecuteSqlCommand(query);
                DailyDungeonEntities.GetContext().SaveChanges();
                MessageBox.Show($"Звичку успішно відредаговано!");
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
