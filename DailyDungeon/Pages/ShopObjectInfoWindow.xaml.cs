using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

            moneyCount = DataBaseModel.GetUserMoneyCount(username);
            if (moneyCount < 50) buyButton.IsEnabled = false;

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

            moneyCount = DataBaseModel.GetUserMoneyCount(username);
            if (moneyCount < 100) buyButton.IsEnabled = false;

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
                    DataBaseModel.BuyBackground(username, background);
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
                    DataBaseModel.BuyAvatar(username, avatar);
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
