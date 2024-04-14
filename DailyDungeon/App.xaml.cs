using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DailyDungeon
{
    public partial class App : Application
    {
        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            using (var context = new DailyDungeonEntities())
            {
                CheckBox checkBox = sender as CheckBox;

                if (checkBox.DataContext is tasks task)
                {
                    if (checkBox.IsChecked == true)
                    {
                        task.is_done = true;
                    }
                    else
                    {
                        task.is_done = false;
                    }

                    context.Entry(task).State = EntityState.Modified;
                }
                if (checkBox.DataContext is habits habit)
                {
                    if (checkBox.IsChecked == true)
                    {
                        habit.is_done = true;
                    }
                    else
                    {
                        habit.is_done = false;
                    }

                    context.Entry(habit).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
        }
    }
}
