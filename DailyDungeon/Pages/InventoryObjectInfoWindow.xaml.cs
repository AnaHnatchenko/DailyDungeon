using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        public string username { get; set; }
        public Color color { get; set; }
        public ImageSource image { get; set; }

        public InventoryObjectInfoWindow(string userName, Color Color)
        {
            InitializeComponent();
            color = Color;
            objectImage.Fill = new SolidColorBrush(color);
            username = userName;
            Color backgroundColor;
            using (var context = new DailyDungeonEntities())
            {
                backgroundColor = (Color)ColorConverter.ConvertFromString(context.backgrounds.Where(b => b.login_user == username && b.is_used).Select(b => b.background_color).FirstOrDefault());
            }
            if (backgroundColor == color)
            {
                useButton.IsEnabled = false;
            }
        }

        public InventoryObjectInfoWindow(string userName, ImageSource Image)
        {
            InitializeComponent();
            image = Image;
            objectImage.Fill = new ImageBrush(image);
            objectName.Text = "Аватар";
            objectDescription.Text = "Ви можете використати цей аватар для зміни фото вашого акаунту на відповідне зображення";
            username = userName;
            string avatarImage;
            using (var context = new DailyDungeonEntities())
            {
                avatarImage = context.avatars.Where(a => a.login_user == username && a.is_used).Select(a => a.image_source).FirstOrDefault();
            }
            if (avatarImage == image.ToString())
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
                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        string comparableColor = color.ToString();
                        var newBackground = context.backgrounds.FirstOrDefault(b => b.login_user == username && b.background_color == comparableColor);
                        if (newBackground == null) newBackground = context.backgrounds.FirstOrDefault(b => b.login_user == username && b.background_color == "#623ed0");
                        
                        var background = context.backgrounds.FirstOrDefault(b => b.login_user == username && b.is_used);
                        background.is_used = false;
                        newBackground.is_used = true;
                        context.SaveChanges();
                        MessageBox.Show("Фон успішно використано!");
                    }
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
                    using (var context = new DailyDungeonEntities())
                    {
                        string comparableImage = image.ToString();
                        var newAvatar = context.avatars.FirstOrDefault(a => a.login_user == username && a.image_source == comparableImage);
                        if (newAvatar != null)
                        {
                            var avatar = context.avatars.FirstOrDefault(a => a.login_user == username && a.is_used);
                            avatar.is_used = false;
                            newAvatar.is_used = true;
                            context.SaveChanges();
                            MessageBox.Show("Аватар успішно використано!");
                        }
                    }
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
