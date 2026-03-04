using System.Linq;
using System.Windows;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Windows.Media;

namespace DailyDungeon
{
    public static class DataBaseModel
    {
        public static void ReloadData()
        {
            using (var context = new DailyDungeonEntities())
            {
                context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            }
        }

        public static bool IsLoginExists(string login)
        {
            using (var context = new DailyDungeonEntities())
            {
                bool userExists = context.users.Any(u => u.login_user == login);
                return userExists;
            }
        }

        public static bool IsUserExists(string login, string password)
        {
            using (var context = new DailyDungeonEntities())
            {
                bool userExists = context.users.Any(u => u.login_user == login && u.password_user == password);
                return userExists;
            }
        }

        public static void CreateNewAccount(users newUser, avatars avatar, backgrounds background)
        {
            using (var context = new DailyDungeonEntities())
            {
                context.users.Add(newUser);
                context.avatars.Add(avatar);
                context.backgrounds.Add(background);
                context.SaveChanges();

                MessageBox.Show("Акаунт успішно створено!");
            }
        }

        public static int GetUserMoneyCount(string username)
        {
            using (var context = new DailyDungeonEntities())
            {
                var user = context.users.FirstOrDefault(u => u.login_user == username);
                return user != null ? user.money_count : 0;
            }
        }

        public static Color GetBackgroundColor(string username)
        {
            using (var context = new DailyDungeonEntities())
            {
                var colorString = context.backgrounds.Where(b => b.login_user == username && b.is_used).Select(b => b.background_color).FirstOrDefault();

                if (colorString != null) return (Color)ColorConverter.ConvertFromString(colorString);
                else return Colors.White;
            }
        }

        public static void BuyBackground(string username, backgrounds background)
        {
            using (var context = new DailyDungeonEntities())
            {
                var user = context.users.FirstOrDefault(u => u.login_user == username);
                if (user != null)
                {
                    user.money_count -= 50;
                    context.backgrounds.Add(background);
                    context.SaveChanges();
                    MessageBox.Show("Фон успішно придбано та додано до інвентарю!");
                }
            }
        }

        public static void SetBackgroundColor(string username, Color color)
        {
            string comparableColor = color.ToString();

            using (var context = new DailyDungeonEntities())
            {
                var newBackground = context.backgrounds.FirstOrDefault(b => b.login_user == username && b.background_color == comparableColor);
                if (newBackground == null) newBackground = context.backgrounds.FirstOrDefault(b => b.login_user == username && b.background_color == "#623ed0");

                var currentBackground = context.backgrounds.FirstOrDefault(b => b.login_user == username && b.is_used);
                if (currentBackground != null) currentBackground.is_used = false;

                if (newBackground != null) newBackground.is_used = true;

                context.SaveChanges();
                MessageBox.Show("Фон успішно використано!");
            }
        }

        public static string GetAvatarImage(string username)
        {
            using (var context = new DailyDungeonEntities())
            {
                string imagePath = context.avatars.Where(a => a.login_user == username && a.is_used).Select(a => a.image_source).FirstOrDefault();
                return $"pack://application:,,,{imagePath}";
            }
        }

        public static void BuyAvatar(string username, avatars avatar)
        {
            using (var context = new DailyDungeonEntities())
            {
                var user = context.users.FirstOrDefault(u => u.login_user == username);
                if (user != null)
                {
                    user.money_count -= 100;
                    context.avatars.Add(avatar);
                    context.SaveChanges();
                    MessageBox.Show("Аватар успішно придбано та додано до інвентарю!");
                }
            }
        }

        public static void SetAvatarImage(string username, string image)
        {
            using (var context = new DailyDungeonEntities())
            {
                var newAvatar = context.avatars.FirstOrDefault(a => a.login_user == username && a.image_source == image);
                if (newAvatar != null)
                {
                    var currentAvatar = context.avatars.FirstOrDefault(a => a.login_user == username && a.is_used);
                    if (currentAvatar != null) currentAvatar.is_used = false;

                    newAvatar.is_used = true;
                    context.SaveChanges();
                    MessageBox.Show("Аватар успішно використано!");
                }
            }
        }

        public static List<avatars> GetAvatarsList(string username)
        {
            using (var context = new DailyDungeonEntities())
            {
                return context.avatars.Where(a => a.login_user == username).ToList();
            }
        }

        public static List<string> GetUsedAvatars(string username)
        {
            using (var context = new DailyDungeonEntities())
            {
                return context.avatars.Where(i => i.login_user == username).Select(i => i.image_source).ToList();
            }
        }

        public static List<backgrounds> GetBackgroundsList(string username)
        {
            using (var context = new DailyDungeonEntities())
            {
                return context.backgrounds.Where(a => a.login_user == username).ToList();
            }
        }

        public static List<tasks> GetTasksForUser(string username)
        {
            using (var context = new DailyDungeonEntities())
            {
                return context.tasks.Where(t => t.login_user == username).ToList();
            }
        }

        public static List<habits> GetHabitsForUser(string username)
        {
            using (var context = new DailyDungeonEntities())
            {
                return context.habits.Where(h => h.login_user == username).ToList();
            }
        }

        public static void CreateTask(tasks newTask)
        {
            using (var context = new DailyDungeonEntities())
            {
                context.tasks.Add(newTask);
                context.SaveChanges();
            }
        }

        public static void CreateHabit(habits newHabit)
        {
            using (var context = new DailyDungeonEntities())
            {
                context.habits.Add(newHabit);
                context.SaveChanges();
            }
        }

        public static void EditTask(tasks task)
        {
            using (var context = new DailyDungeonEntities())
            {
                var taskToUpdate = context.tasks.FirstOrDefault(t => t.id_task == task.id_task);
                if (taskToUpdate != null)
                {
                    taskToUpdate.login_user = task.login_user;
                    taskToUpdate.name_task = task.name_task.Trim();
                    taskToUpdate.description_task = string.IsNullOrWhiteSpace(task.description_task) ? string.Empty : task.description_task.Trim();
                    taskToUpdate.complexity_task = task.complexity_task;
                    taskToUpdate.tag_task = string.IsNullOrWhiteSpace(task.tag_task) ? string.Empty : task.tag_task.Trim();
                    taskToUpdate.deadline_task = task.deadline_task;
                    taskToUpdate.is_done = false;

                    context.SaveChanges();
                    MessageBox.Show("Завдання успішно відредаговано!");
                }
                else
                {
                    MessageBox.Show("Завдання не знайдено.");
                }
            }
        }

        public static void EditHabit(habits habit)
        {
            using (var context = new DailyDungeonEntities())
            {
                var habitToUpdate = context.habits.FirstOrDefault(h => h.id_habit == habit.id_habit);
                if (habitToUpdate != null)
                {
                    habitToUpdate.login_user = habit.login_user;
                    habitToUpdate.name_habit = habit.name_habit.Trim();
                    habitToUpdate.description_habit = string.IsNullOrWhiteSpace(habit.description_habit) ? string.Empty : habit.description_habit.Trim();
                    habitToUpdate.complexity_habit = habit.complexity_habit;
                    habitToUpdate.type_habit = habit.type_habit;
                    habitToUpdate.tag_habit = string.IsNullOrWhiteSpace(habit.tag_habit) ? string.Empty : habit.tag_habit.Trim();

                    context.SaveChanges();
                    MessageBox.Show("Звичку успішно відредаговано!");
                }
                else
                {
                    MessageBox.Show("Звичку не знайдено");
                }
            }
        }

        public static void DoTask(tasks task)
        {
            if (task == null)
            {
                MessageBox.Show("Будь ласка, оберіть завдання для виконання.");
                return;
            }

            if (!task.is_done)
            {
                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        task.is_done = true;
                        context.Entry(task).State = EntityState.Modified;

                        string username = task.login_user;
                        users user = context.users.FirstOrDefault(u => u.login_user == username);
                        if (user != null)
                        {
                            user.money_count += task.ExecutionCost();
                            context.Entry(user).State = EntityState.Modified;
                            if (MessageBox.Show("Чи бажаєте ви видалити це завдання після виконання?", "Вітаю!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                tasks taskToDelete = context.tasks.FirstOrDefault(t => t.id_task == task.id_task);
                                if (taskToDelete != null)
                                {
                                    context.tasks.Remove(taskToDelete);
                                }
                            }
                        }
                        context.SaveChanges();
                        MessageBox.Show("Ви виконали завдання! Винагорода успішно зарахована!", "Вітаю!", MessageBoxButton.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при виконанні завдання: {ex.Message}");
                }
            }
        }

        public static void DoHabit(habits habit)
        {
            if (habit == null)
            {
                MessageBox.Show("Будь ласка, оберіть звичку для виконання.");
                return;
            }

            if (!habit.is_done)
            {
                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        habit.is_done = true;
                        context.Entry(habit).State = EntityState.Modified;

                        string username = habit.login_user;
                        users user = context.users.FirstOrDefault(u => u.login_user == username);
                        if (user != null)
                        {
                            user.money_count += habit.ExecutionCost();
                            context.Entry(user).State = EntityState.Modified;
                        }
                        context.SaveChanges();
                        MessageBox.Show("Ви виконали звичку! Винагорода успішно зарахована!", "Вітаю!", MessageBoxButton.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при виконанні звички: {ex.Message}");
                }
            }
        }

        public static void UnDoTask(tasks task)
        {
            if (task == null)
            {
                MessageBox.Show("Будь ласка, оберіть завдання для виконання.");
                return;
            }

            if (task.is_done)
            {
                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        task.is_done = false;
                        context.Entry(task).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при виконанні завдання: {ex.Message}");
                }
            }
        }

        public static void UnDoHabit(habits habit)
        {
            if (habit == null)
            {
                MessageBox.Show("Будь ласка, оберіть звичку для виконання.");
                return;
            }

            if (habit.is_done)
            {
                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        habit.is_done = false;
                        context.Entry(habit).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при виконанні звички: {ex.Message}");
                }
            }
        }

        public static void DeleteTask(string username, tasks selectedTask)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Будь ласка, оберіть завдання для видалення.");
                return;
            }

            if (MessageBox.Show("Ви дійсно бажаєте видалити це завдання?", "Увага", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        var taskToDelete = context.tasks.FirstOrDefault(t => t.id_task == selectedTask.id_task && t.login_user == username);
                        if (taskToDelete != null)
                        {
                            context.tasks.Remove(taskToDelete);
                            context.SaveChanges();
                            MessageBox.Show("Завдання видалено.");
                        }
                        else
                        {
                            MessageBox.Show("Завдання не знайдено або вже було видалено.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при видаленні завдання: {ex.Message}");
                }
            }
        }

        public static void DeleteHabit(string username, habits selectedHabit)
        {
            if (selectedHabit == null)
            {
                MessageBox.Show("Будь ласка, оберіть звичку для видалення.");
                return;
            }

            if (MessageBox.Show("Ви дійсно бажаєте видалити цю звичку?", "Увага", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var context = new DailyDungeonEntities())
                    {
                        var habitToDelete = context.habits.FirstOrDefault(h => h.id_habit == selectedHabit.id_habit && h.login_user == username);
                        if (habitToDelete != null)
                        {
                            context.habits.Remove(habitToDelete);
                            context.SaveChanges();
                            MessageBox.Show("Звичку видалено.");
                        }
                        else
                        {
                            MessageBox.Show("Звичку не знайдено або вже було видалено.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Виникла помилка при видаленні звички: {ex.Message}");
                }
            }
        }
    }
}
