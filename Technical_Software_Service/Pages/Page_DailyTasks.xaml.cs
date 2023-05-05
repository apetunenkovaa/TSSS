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
            // Получаем выбранную задачу
            var selectedTask = (sender as Button).DataContext as DailyTasks;

            var RewardscoreTask = selectedTask.Score; // Получаем score для выполненной задачи из базы данных

            var RewardscoreXP = selectedTask.XP; // Получаем XP для выполненной задачи из базы данных

            // Получаем текущего пользователя из поля user
            var currentUser = user;

            // Получаем запись пользователя для выбранной задачи
            var userDailyTask = DataBase.Base.UserDailyTasks.FirstOrDefault(t => t.UserId == currentUser.Id && t.DailyTasksID == selectedTask.Id);

            if (userDailyTask != null)
            {
                // Обновляем поле IsCompleted
                userDailyTask.IsCompleted = true;

                // Обновляем поле CompletionDate
                userDailyTask.CompletionDate = DateTime.Now;

                // Увеличиваем счетчик выполненных задач
                userDailyTask.CompletedCount++;

                // Начисляем XP и Score пользователю
                currentUser.XP += RewardscoreXP;
                currentUser.Score += RewardscoreTask;

                // Обновляем запись в базе данных
                DataBase.Base.Entry(userDailyTask).State = EntityState.Modified;
                DataBase.Base.Entry(currentUser).State = EntityState.Modified;
                DataBase.Base.SaveChanges();

                // Обновляем количество выполненных задач в соответствующей задаче
                selectedTask.CompletedCount++;

                // Обновляем запись в базе данных
                DataBase.Base.Entry(selectedTask).State = EntityState.Modified;
                DataBase.Base.SaveChanges();

                // Обновляем отображение данных в окне
                lstDailyTasks.ItemsSource = DataBase.Base.DailyTasks.ToList();
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
                    // Удаляем элемент из источника данных
                    DataBase.Base.DailyTasks.Remove(selectedItem);
                    DataBase.Base.SaveChanges();

                    // Обновляем содержимое ListView
                    lstDailyTasks.ItemsSource = DataBase.Base.DailyTasks.ToList();
                }
            }
        }
    }
}
