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
            // Получаем объект задания, соответствующий нажатой кнопке "Завершить"
            var task = (sender as FrameworkElement)?.DataContext as DailyTasks;

            if (task != null)
            {
                // Проверяем, было ли задание уже выполнено пользователем
                var userTask = user.UserDailyTasks.FirstOrDefault(x => x.DailyTasksID == task.Id);
                if (userTask != null && userTask.IsCompleted)
                {
                    MessageBox.Show("Это задание уже выполнено!");
                }
                else
                {
                    // Помечаем задание как завершенное
                    task.IsCompleted = true;

                    // Находим связанный с пользователем объект UserDailyTasks и обновляем его
                    if (userTask != null)
                    {
                        userTask.IsCompleted = true;
                        userTask.CompletionDate = DateTime.Now;

                        // Начисляем пользователю XP и score за выполненную задачу
                        user.XP += task.XP;
                        user.Score += task.Score;

                        DataBase.Base.SaveChanges();
                    }

                    // Обновляем список заданий
                    lstDailyTasks.ItemsSource = DataBase.Base.DailyTasks.ToList();
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
