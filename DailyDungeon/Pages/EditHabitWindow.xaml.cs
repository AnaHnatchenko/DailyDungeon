using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

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
            if (string.IsNullOrWhiteSpace(habit.name_habit))
            {
                MessageBox.Show("Не вдалося створити завдання! Обов'язково введіть його назву");
                return;
            }
            
            try
            {
                DataBaseModel.EditHabit(habit);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка при редагуванні звички: {ex.Message}");
            }

            this.Hide();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
