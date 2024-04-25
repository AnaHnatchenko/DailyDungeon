using System;
using System.Windows;

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
           
            try
            {
                habit.login_user = username;
                habit.name_habit = habit.name_habit.Trim();
                habit.description_habit = string.IsNullOrWhiteSpace(habit.description_habit) ? string.Empty : habit.description_habit.Trim();
                habit.tag_habit = string.IsNullOrWhiteSpace(habit.tag_habit) ? string.Empty : habit.tag_habit.Trim();
                habit.is_done = false;

                DataBaseModel.CreateHabit(habit);
                MessageBox.Show("Звичку успішно створено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка при створенні звички: {ex.Message}");
            }

            this.Hide();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
