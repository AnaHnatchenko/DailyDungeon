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
using static System.Net.Mime.MediaTypeNames;

namespace DailyDungeon.Pages
{
    public partial class ShopObjectInfoWindow : Window
    {
        public event EventHandler<Border> BuyObject;
        public Border Sender { get; set; }
        public string username { get; set; }
        public int moneyCount { get; set; }

        public ShopObjectInfoWindow(Border sender, string userName, Color color)
        {
            InitializeComponent();
            objectImage.Fill = new SolidColorBrush(color);
            username = userName;

            this.DataContext = this;
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

            Sender = sender;
        }

        public ShopObjectInfoWindow(Border sender, string userName, ImageSource image)
        {
            InitializeComponent();
            objectImage.Fill = new ImageBrush(image);
            objectName.Text = "Аватар";
            objectDescription.Text = "Ви можете використати цей аватар для зміни фото вашого акаунту на відповідне зображення";
            username = userName;

            this.DataContext = this;
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

            Sender = sender;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            if (objectImage.Fill is SolidColorBrush solidColorBrush)
            {
                Color color = solidColorBrush.Color;
                backgrounds background = new backgrounds()
                {
                    login_user = username,
                    background_color = color.ToString(),
                    is_used = false
                };
                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        var user = context.users.FirstOrDefault(u => u.login_user == username);
                        user.money_count = moneyCount - 50;
                        context.backgrounds.Add(background);
                        context.SaveChanges();

                        MessageBox.Show("Фон успішно придбано та додано до інвентарю!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при покупці фону: {ex.Message}");
                }
            }
            else
            {
                ImageSource image = ((ImageBrush)objectImage.Fill).ImageSource;
                avatars avatar = new avatars()
                {
                    login_user = username,
                    image_source = image.ToString(),
                    is_used = false
                };
                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        var user = context.users.FirstOrDefault(u => u.login_user == username);
                        user.money_count = moneyCount - 100;
                        context.avatars.Add(avatar);
                        context.SaveChanges();

                        MessageBox.Show("Аватар успішно придбано та додано до інвентарю!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при покупці аватару: {ex.Message}");
                }
            }

            BuyObject?.Invoke(this, Sender);
            this.Hide();
        }
    }
}
