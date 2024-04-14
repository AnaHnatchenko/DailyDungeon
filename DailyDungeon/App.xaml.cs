using DailyDungeon.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DailyDungeon
{
    public partial class App : Application
    {
        public void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            string username;
            users user;
            if (checkBox != null)
            {
                if (checkBox.DataContext is tasks task)
                {
                    if (!task.is_done)
                    {
                        using (var context = new DailyDungeonEntities())
                        {
                            task.is_done = true;
                            context.Entry(task).State = EntityState.Modified;

                            username = task.login_user;
                            user = context.users.Where(u => u.login_user == username).FirstOrDefault();
                            if (user != null)
                            {
                                user.money_count = user.money_count + task.ExecutionCost();
                                context.Entry(user).State = EntityState.Modified;
                                if (MessageBox.Show("Чи бажаєте ви видалити це завдання після виконання?", "Вітаю!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    tasks taskToDelete = context.tasks.Where(t => t.id_task == task.id_task).FirstOrDefault();
                                    context.tasks.Remove(taskToDelete);
                                }
                            }
                            context.SaveChanges();
                            MessageBox.Show("Ви виконали завдання! Винагорода успішно зарахована!", "Вітаю!", MessageBoxButton.OK);
                        }    
                    }    
                }
                if (checkBox.DataContext is habits habit)
                {
                    if (!habit.is_done)
                    {
                        using (var context = new DailyDungeonEntities())
                        {
                            habit.is_done = true;
                            context.Entry(habit).State = EntityState.Modified;

                            username = habit.login_user;
                            user = context.users.Where(u => u.login_user == username).FirstOrDefault();
                            if (user != null)
                            {
                                user.money_count = user.money_count + habit.ExecutionCost();
                                context.Entry(user).State = EntityState.Modified;
                            }
                            context.SaveChanges();
                            MessageBox.Show("Ви виконали звичку! Винагорода успішно зарахована!", "Вітаю!", MessageBoxButton.OK);
                        }
                    } 
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                if (checkBox.DataContext is tasks task)
                {
                    if (task.is_done)
                    {
                        using (var context = new DailyDungeonEntities())
                        {
                            task.is_done = false;
                            context.Entry(task).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }
                if (checkBox.DataContext is habits habit)
                {
                    if (habit.is_done)
                    {
                        using (var context = new DailyDungeonEntities())
                        {
                            habit.is_done = false;
                            context.Entry(habit).State = EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                }
            }
                
        }
    }
}
