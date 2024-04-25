using System;
using System.Windows;
using System.Windows.Media;

namespace DailyDungeon.Pages
{
    public partial class InventoryObjectInfoWindow : Window
    {
        public string username { get; set; }
        public Color color { get; set; }
        public ImageSource image { get; set; }

        public InventoryObjectInfoWindow(string userName, Color Color)
        {
            InitializeComponent();
            color = Color;
            objectImage.Fill = new SolidColorBrush(color);
            username = userName;

            Color backgroundColor = DataBaseModel.GetBackgroundColor(username);
            if (backgroundColor == color) useButton.IsEnabled = false;
        }

        public InventoryObjectInfoWindow(string userName, ImageSource Image)
        {
            InitializeComponent();
            image = Image;
            objectImage.Fill = new ImageBrush(image);
            objectName.Text = "Аватар";
            objectDescription.Text = "Ви можете використати цей аватар для зміни фото вашого акаунту на відповідне зображення";
            username = userName;

            string avatarImage = DataBaseModel.GetAvatarImage(username);
            if (avatarImage == image.ToString()) useButton.IsEnabled = false;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Use_Click(object sender, RoutedEventArgs e)
        {
            if (objectImage.Fill is SolidColorBrush solidColorBrush)
            {
                try
                {
                    DataBaseModel.SetBackgroundColor(username, color);
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при використанні фону: {ex.Message}");
                }
            }
            if (objectImage.Fill is ImageBrush imageBrush)
            {
                try
                {
                    DataBaseModel.SetAvatarImage(username, image.ToString());
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при використанні аватару: {ex.Message}");
                }
            }  
        }
    }
}
