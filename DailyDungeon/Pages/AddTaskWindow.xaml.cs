using System;
using System.Collections.Generic;
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
    public partial class AddTaskWindow : Window
    {
        private readonly string[] taskComplexity = {"Легке", "Середнє", "Складне"};
        private readonly string[] taskTags = { "Робота", "Навчання", "Здоров'я", "Хобі"};

        public AddTaskWindow()
        {
            InitializeComponent();
            this.Deactivated += ModalWindow_Deactivated;
            tagsComboBox.ItemsSource = taskTags;
            complexityComboBox.ItemsSource = taskComplexity;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
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
