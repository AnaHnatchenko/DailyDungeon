using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var converter = new BrushConverter();
            var members = new ObservableCollection<Member>();

            //Create DataGrid Item Info
            members.Add(new Member() { Number = "1", Character = "J", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "John Doe", Position = "Coach", Email = "john.doe@gmail.com", Phone = "415-954-1475" });
            members.Add(new Member() { Number = "2", Character = "R", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "Reza Alavi", Position = "Administrator", Email = "rezal1@hotmail.com", Phone = "254-415-63455" });
            members.Add(new Member() { Number = "3", Character = "D", BgColor = (Brush)converter.ConvertFromString("#ff8f00"), Name = "Dennis Castillo", Position = "Coach", Email = "dent.cast@gmail.com", Phone = "865-324-2456" });
            members.Add(new Member() { Number = "4", Character = "G", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "Gabriel Cox", Position = "Coach", Email = "coxcox@gmail.com", Phone = "834-441-5274" });
            members.Add(new Member() { Number = "5", Character = "L", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "Lena Jones", Position = "Manager", Email = "len.off@gmail.com", Phone = "132-624-8261" });
            members.Add(new Member() { Number = "6", Character = "B", BgColor = (Brush)converter.ConvertFromString("#6741d9"), Name = "Benjamin Caliword", Position = "Administrator", Email = "beni12@hotmail.com", Phone = "271-194-5276" });
            members.Add(new Member() { Number = "7", Character = "S", BgColor = (Brush)converter.ConvertFromString("#ff6d00"), Name = "Sophia Muris", Position = "Coach", Email = "sophi.muri@gmail.com", Phone = "672-619-8236" });
            members.Add(new Member() { Number = "8", Character = "A", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "Ali Pormand", Position = "Manager", Email = "alipor@yahoo.com", Phone = "284-563-1523" });
            members.Add(new Member() { Number = "9", Character = "F", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "Frank underwood", Position = "Manager", Email = "frank@yahoo.com", Phone = "945-645-4235" });
            members.Add(new Member() { Number = "10", Character = "S", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "Saeed Dasman", Position = "Coach", Email = "saeed.dasi@hotmail.com", Phone = "756-724-8368" });

            members.Add(new Member() { Number = "1", Character = "J", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "John Doe", Position = "Coach", Email = "john.doe@gmail.com", Phone = "415-954-1475" });
            members.Add(new Member() { Number = "2", Character = "R", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "Reza Alavi", Position = "Administrator", Email = "rezal1@hotmail.com", Phone = "254-415-63455" });
            members.Add(new Member() { Number = "3", Character = "D", BgColor = (Brush)converter.ConvertFromString("#ff8f00"), Name = "Dennis Castillo", Position = "Coach", Email = "dent.cast@gmail.com", Phone = "865-324-2456" });
            members.Add(new Member() { Number = "4", Character = "G", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "Gabriel Cox", Position = "Coach", Email = "coxcox@gmail.com", Phone = "834-441-5274" });
            members.Add(new Member() { Number = "5", Character = "L", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "Lena Jones", Position = "Manager", Email = "len.off@gmail.com", Phone = "132-624-8261" });
            members.Add(new Member() { Number = "6", Character = "B", BgColor = (Brush)converter.ConvertFromString("#6741d9"), Name = "Benjamin Caliword", Position = "Administrator", Email = "beni12@hotmail.com", Phone = "271-194-5276" });
            members.Add(new Member() { Number = "7", Character = "S", BgColor = (Brush)converter.ConvertFromString("#ff6d00"), Name = "Sophia Muris", Position = "Coach", Email = "sophi.muri@gmail.com", Phone = "672-619-8236" });
            members.Add(new Member() { Number = "8", Character = "A", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "Ali Pormand", Position = "Manager", Email = "alipor@yahoo.com", Phone = "284-563-1523" });
            members.Add(new Member() { Number = "9", Character = "F", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "Frank underwood", Position = "Manager", Email = "frank@yahoo.com", Phone = "945-645-4235" });
            members.Add(new Member() { Number = "10", Character = "S", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "Saeed Dasman", Position = "Coach", Email = "saeed.dasi@hotmail.com", Phone = "756-724-8368" });

            members.Add(new Member() { Number = "1", Character = "J", BgColor = (Brush)converter.ConvertFromString("#1098ad"), Name = "John Doe", Position = "Coach", Email = "john.doe@gmail.com", Phone = "415-954-1475" });
            members.Add(new Member() { Number = "2", Character = "R", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "Reza Alavi", Position = "Administrator", Email = "rezal1@hotmail.com", Phone = "254-415-63455" });
            members.Add(new Member() { Number = "3", Character = "D", BgColor = (Brush)converter.ConvertFromString("#ff8f00"), Name = "Dennis Castillo", Position = "Coach", Email = "dent.cast@gmail.com", Phone = "865-324-2456" });
            members.Add(new Member() { Number = "4", Character = "G", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "Gabriel Cox", Position = "Coach", Email = "coxcox@gmail.com", Phone = "834-441-5274" });
            members.Add(new Member() { Number = "5", Character = "L", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "Lena Jones", Position = "Manager", Email = "len.off@gmail.com", Phone = "132-624-8261" });
            members.Add(new Member() { Number = "6", Character = "B", BgColor = (Brush)converter.ConvertFromString("#6741d9"), Name = "Benjamin Caliword", Position = "Administrator", Email = "beni12@hotmail.com", Phone = "271-194-5276" });
            members.Add(new Member() { Number = "7", Character = "S", BgColor = (Brush)converter.ConvertFromString("#ff6d00"), Name = "Sophia Muris", Position = "Coach", Email = "sophi.muri@gmail.com", Phone = "672-619-8236" });
            members.Add(new Member() { Number = "8", Character = "A", BgColor = (Brush)converter.ConvertFromString("#ff5252"), Name = "Ali Pormand", Position = "Manager", Email = "alipor@yahoo.com", Phone = "284-563-1523" });
            members.Add(new Member() { Number = "9", Character = "F", BgColor = (Brush)converter.ConvertFromString("#1e88e5"), Name = "Frank underwood", Position = "Manager", Email = "frank@yahoo.com", Phone = "945-645-4235" });
            members.Add(new Member() { Number = "10", Character = "S", BgColor = (Brush)converter.ConvertFromString("#0ca678"), Name = "Saeed Dasman", Position = "Coach", Email = "saeed.dasi@hotmail.com", Phone = "756-724-8368" });

            membersDataGrid.ItemsSource = members;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;
                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized = true;
                }
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Hide();
        }

        private void CloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
            Application.Current.Shutdown();
        }
    }

    public class Member
    {
        public string Character { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Brush BgColor { get; set; }
    }
}
