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
            lstDailyTasks.ItemsSource = DataBase.Base.DailyTasks.ToList();

            if (user.Roles.Kind == "Администратор")
            {
                btnAdd.Visibility = Visibility.Visible;
            }
            if (user.Roles.Kind == "Администратор")
            {
                btnDelete.Visibility = Visibility.Visible;
            }
            if (user.Roles.Kind == "Администратор")
            {
                btnGiveRandomTasksUsers.Visibility = Visibility.Visible;
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

        }
    }
}
