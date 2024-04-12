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
    public partial class ShopObjectInfoWindow : Window
    {
        public string username { get; set; }

        public ShopObjectInfoWindow(string userName, Color color)
        {
            InitializeComponent();
            objectImage.Fill = new SolidColorBrush(color);
            username = "Anastasia";

            this.DataContext = this;
            int moneyCount;
            using (var context = new DailyDungeonEntities())
            {
                var user = context.users.FirstOrDefault(u => u.login_user == username);
                if (user != null)
                {
                    moneyCount = user.money_count;
                }
                else
                {
                    moneyCount = 0;
                }
            }
            if (moneyCount < 50) 
            {
                buyButton.IsEnabled = false;
            }
        }

        public ShopObjectInfoWindow(string userName, BitmapImage image)
        {
            InitializeComponent();
            objectImage.Fill = new ImageBrush(image);
            objectName.Text = "Аватар";
            objectDescription.Text = "Ви можете використати цей аватар для зміни фото вашого акаунту на відповідне зображення";
            username = "Anastasia";

            this.DataContext = this;
            int moneyCount;
            using (var context = new DailyDungeonEntities())
            {
                var user = context.users.FirstOrDefault(u => u.login_user == username);
                if (user != null)
                {
                    moneyCount = user.money_count;
                }
                else
                {
                    moneyCount = 0;
                }
            }
            if (moneyCount < 100)
            {
                buyButton.IsEnabled = false;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
