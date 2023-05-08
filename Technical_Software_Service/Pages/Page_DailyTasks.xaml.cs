using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Technical_Software_Service.Classes;

namespace Technical_Software_Service
{
    /// <summary>
    /// Логика взаимодействия для Page_DailyTasks.xaml
    /// </summary>
    public partial class Page_DailyTasks : Page
    {
        Users user;
        public Page_DailyTasks(Users user)
        {
            InitializeComponent();
            this.user = user;
            if (user.Roles.Kind == "Администратор")
            {
                lstDailyTasks.ItemsSource = DataBase.Base.DailyTasks.ToList();
                btnAdd.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
                btnGiveRandomTasksUsers.Visibility = Visibility.Visible;
            }
            else
            {
                lstDailyTasks.ItemsSource = user.UserDailyTasks.Select(udt => udt.DailyTasks).ToList();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.MainF.Navigate(new Page_Anything(user));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window_AddDailyTasks dailyTasks = new Window_AddDailyTasks(user);
            dailyTasks.ShowDialog();

            // Обновляем содержимое ListView в основном окне
            lstDailyTasks.ItemsSource = HelpdeskEntities.GetContext().DailyTasks.ToList();
        }

        private void bFinishTask_Click(object sender, RoutedEventArgs e)
        {
            if (user.CompletedCountTickets == 0)
            {
                MessageBox.Show("У вас пока нет выполненных заявок, выполните хотя бы одну, чтобы получить доступ к ежедневным заданиям.");
                return;
            }

            // Получаем идентификатор задачи из параметра команды
            int taskId = (int)((Button)sender).CommandParameter;

            // Ищем задачу по идентификатору в коллекции
            DailyTasks task = DataBase.Base.DailyTasks.FirstOrDefault(t => t.Id == taskId);

            if (task != null)
            {
                var userDailyTask = user.UserDailyTasks.FirstOrDefault(udt => udt.DailyTasksID == task.Id);
                if (user.CompletedCountTickets >= task.TotalCount && userDailyTask == null)
                {
                    user.Score += task.Score;
                    user.XP += task.XP;
                    user.UserDailyTasks.Add(new UserDailyTasks { DailyTasksID = task.Id, IsCompleted = true });
                    MessageBox.Show($"Вы получили {task.Score} очков и {task.XP} опыта за выполнение ежедневного задания \"{task.Title}\"");

                    DataBase.Base.SaveChanges();
                }
                else if (userDailyTask != null)
                {
                    MessageBox.Show($"Вы уже выполнили задание \"{task.Title}\".");
                }
                else
                {
                    MessageBox.Show($"У вас недостаточно выполненных заявок для выполнения задания \"{task.Title}\".");
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный элемент
            var selectedItem = lstDailyTasks.SelectedItem as DailyTasks;
            if (selectedItem != null)
            {
                // Запрашиваем подтверждение удаления
                var result = MessageBox.Show($"Вы уверены, что хотите удалить задание '{selectedItem.Title}'?", "Удаление задания", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Удаляем связанные записи из таблицы UserDailyTasks
                    var tasksToDelete = DataBase.Base.UserDailyTasks.Where(x => x.DailyTasksID == selectedItem.Id);
                    DataBase.Base.UserDailyTasks.RemoveRange(tasksToDelete);

                    // Удаляем элемент из таблицы DailyTasks
                    DataBase.Base.DailyTasks.Remove(selectedItem);

                    try
                    {
                        // Сохраняем изменения в базе данных
                        DataBase.Base.SaveChanges();

                        // Обновляем содержимое ListView
                        lstDailyTasks.ItemsSource = DataBase.Base.DailyTasks.ToList();
                    }
                    catch (Exception ex)
                    {
                        // Обрабатываем возможные ошибки при сохранении изменений
                        MessageBox.Show($"Ошибка при удалении задания: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void lstDailyTasks_Loaded(object sender, RoutedEventArgs e)
        {
            var lv = (ListView)sender;
            var progressDailyTasks = (TextBlock)lv.FindName("progressDailyTasks");
            if (progressDailyTasks != null)
            {
                progressDailyTasks.DataContextChanged += (s, ev) =>
                {
                    if (ev.NewValue is DailyTasks dailyTask)
                    {
                        int completedCount = user.UserDailyTasks.Count(udt => udt.DailyTasksID == dailyTask.Id && udt.IsCompleted);
                        progressDailyTasks.Text = $"Выполнено: {completedCount} / Всего: {dailyTask.TotalCount}";
                    }
                };
            }
        }

        private void btnGiveRandomTasksUsers_Click(object sender, RoutedEventArgs e)
        {
            // Получаем всех пользователей из базы данных
            var users = DataBase.Base.Users.ToList();

            // Получаем все доступные ежедневные задания из базы данных
            var dailyTasks = DataBase.Base.DailyTasks.ToList();

            // Проходим по каждому пользователю
            foreach (var user in users)
            {
                // Получаем уже назначенные пользователю ежедневные задания
                var assignedTasks = user.UserDailyTasks.Where(udt => udt.IsCompleted == false).Select(udt => udt.DailyTasksID).ToList();

                // Получаем список доступных ежедневных заданий, которые еще не были назначены пользователю
                var availableTasks = dailyTasks.Where(dt => !assignedTasks.Contains(dt.Id)).ToList();

                // Если доступных заданий не хватает, то сообщаем об этом пользователю
                if (availableTasks.Count < 3)
                {
                    MessageBox.Show($"Для пользователя {user.UserName} недостаточно доступных заданий.");
                }
                else
                {
                    // Выбираем случайные 3 задания из списка доступных заданий
                    var randomTasks = availableTasks.OrderBy(x => Guid.NewGuid()).Take(3).ToList();

                    // Назначаем выбранные задания пользователю
                    foreach (var task in randomTasks)
                    {
                        user.UserDailyTasks.Add(new UserDailyTasks { DailyTasksID = task.Id, IsCompleted = false });
                    }

                    // Сохраняем изменения в базе данных
                    DataBase.Base.SaveChanges();
                }
            }

            MessageBox.Show("Ежедневные задания успешно назначены пользователям.");
        }
    }
}
