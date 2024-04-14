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
            if (string.IsNullOrWhiteSpace(habit.name_habit))
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
                        var habitToUpdate = context.habits.FirstOrDefault(h => h.id_habit == habit.id_habit);
                        if (habitToUpdate != null)
                        {
                            habitToUpdate.login_user = username;
                            habitToUpdate.name_habit = habit.name_habit.Trim();
                            if (string.IsNullOrWhiteSpace(habit.description_habit)) habitToUpdate.description_habit = String.Empty;
                            else habitToUpdate.description_habit = habit.description_habit.Trim();
                            habitToUpdate.complexity_habit = habit.complexity_habit;
                            habitToUpdate.type_habit = habit.type_habit;
                            if (string.IsNullOrWhiteSpace(habit.tag_habit)) habitToUpdate.tag_habit = String.Empty;
                            else habitToUpdate.tag_habit = habit.tag_habit.Trim();

                            context.SaveChanges();
                            MessageBox.Show($"Звичку успішно відредаговано!");
                        }
                        else
                        {
                            MessageBox.Show("Звичку не знайдено для редагування.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при редагуванні звички: {ex.Message}");
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
