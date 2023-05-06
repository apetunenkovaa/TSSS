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
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.MainF.Navigate(new Page_Anything(user));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window_AddDailyTasks dailyTasks = new Window_AddDailyTasks(user);
            dailyTasks.ShowDialog();
        }

        private void bFinishTask_Click(object sender, RoutedEventArgs e)
        {
            List<DailyTasks> dailyTasks = DataBase.Base.DailyTasks.ToList();

            foreach (var dailyTask in dailyTasks)
            {
                var userDailyTask = user.UserDailyTasks.FirstOrDefault(udt => udt.DailyTasksID == dailyTask.Id);
                if (userDailyTask == null && user.CompletedCountTickets >= dailyTask.TotalCount)
                {
                    user.Score += dailyTask.Score;
                    user.XP += dailyTask.XP;
                    user.UserDailyTasks.Add(new UserDailyTasks { DailyTasksID = dailyTask.Id, IsCompleted = true });
                    MessageBox.Show($"Вы получили {dailyTask.Score} очков и {dailyTask.XP} опыта за выполнение ежедневного задания \"{dailyTask.Title}\"");
                }
                else if (userDailyTask != null)
                {
                    MessageBox.Show($"Вы уже выполнили задание \"{dailyTask.Title}\".");
                }
                else
                {
                    MessageBox.Show($"Вы еще не выполнили достаточное количество билетов для выполнения задания \"{dailyTask.Title}\".");
                }
            }
            DataBase.Base.SaveChanges();
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
                    // Удаляем элемент из источника данных
                    DataBase.Base.DailyTasks.Remove(selectedItem);
                    DataBase.Base.SaveChanges();

                    // Обновляем содержимое ListView
                    lstDailyTasks.ItemsSource = DataBase.Base.DailyTasks.ToList();
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
    }
}
