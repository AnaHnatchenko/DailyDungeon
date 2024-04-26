using System.Windows;
using System.Windows.Controls;

namespace DailyDungeon
{
    public partial class App : Application
    {
        public void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                if (checkBox.DataContext is tasks task) DataBaseModel.DoTask(task);
                if (checkBox.DataContext is habits habit) DataBaseModel.DoHabit(habit);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                if (checkBox.DataContext is tasks task) DataBaseModel.UnDoTask(task);
                if (checkBox.DataContext is habits habit) DataBaseModel.UnDoHabit(habit);
            }
                
        }
    }
}
