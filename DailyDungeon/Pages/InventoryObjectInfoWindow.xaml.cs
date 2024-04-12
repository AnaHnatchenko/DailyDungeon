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
    public partial class InventoryObjectInfoWindow : Window
    {
        public event EventHandler<Color> UseBackgroundObject;
        public event EventHandler<ImageSource> UseAvatarObject;
        public string username { get; set; }
        public Color color { get; set; }
        public ImageSource image { get; set; }

        public InventoryObjectInfoWindow(string userName, Color backgroundColor, Color Color)
        {
            InitializeComponent();
            color = Color;
            objectImage.Fill = new SolidColorBrush(color);
            username = "Anastasia";
            if (backgroundColor == color)
            {
                useButton.IsEnabled = false;
            }
        }

        public InventoryObjectInfoWindow(string userName, ImageSource avatarImage, ImageSource Image)
        {
            InitializeComponent();
            image = Image;
            objectImage.Fill = new ImageBrush(image);
            objectName.Text = "Аватар";
            objectDescription.Text = "Ви можете використати цей аватар для зміни фото вашого акаунту на відповідне зображення";
            username = "Anastasia";
            if (avatarImage.ToString() == image.ToString())
            {
                useButton.IsEnabled = false;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Use_Click(object sender, RoutedEventArgs e)
        {
            if (objectImage.Fill is SolidColorBrush solidColorBrush)
            {
                UseBackgroundObject?.Invoke(this, color);
                this.Hide();
            }
            if (objectImage.Fill is ImageBrush imageBrush)
            {
                UseAvatarObject?.Invoke(this, image);
                this.Hide();
            }  
        }
    }
}
