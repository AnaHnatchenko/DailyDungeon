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

        private void AddHabit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(habit.name_habit))
            {
                MessageBox.Show("Не вдалося створити завдання! Обов'язково введіть його назву");
                return;
            }
            else
            {
                habit.login_user = username;
                habit.name_habit = habit.name_habit.Trim();
                habit.description_habit = habit.description_habit.Trim();
                if (string.IsNullOrWhiteSpace(habit.description_habit)) habit.description_habit = String.Empty;
                else habit.description_habit = habit.description_habit.Trim();
                if (string.IsNullOrWhiteSpace(habit.tag_habit)) habit.tag_habit = String.Empty;
                else habit.tag_habit = habit.tag_habit.Trim();
                habit.is_done = false;

                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        context.habits.Add(habit);
                        context.SaveChanges();

                        MessageBox.Show("Звичку успішно створено!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при створенні звички: {ex.Message}");
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
