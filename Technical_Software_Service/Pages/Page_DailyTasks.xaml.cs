using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
            tbCompleteTicketProgress.Text = $"Закрыто заявок: {user.CompletedCountTicketsClosed}\nСоздано заявок: {user.CreateCountTickets}";

            lstDailyTasks.ItemsSource = DataBase.Base.DailyTasks.ToList();

            //Фильтр заданий
            cbFilter.Items.Add("Все задания");
            cbFilter.Items.Add("Активные задания");
            cbFilter.Items.Add("Выполненные задания");
            cbFilter.SelectedIndex = 0; // Выбираем первый элемент по умолчанию
            cbFilter.SelectionChanged += cbFilter_SelectionChanged;

            //Отображение кнопок
            if (user.Roles.Kind == "Администратор")
            {
                btnAdd.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
                btnEdit.Visibility = Visibility.Visible;
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
            if (user.CompletedCountTicketsClosed == 0 && user.CreateCountTickets == 0)
            {
                MessageBox.Show("У вас пока нет выполненных заявок, выполните хотя бы одну, чтобы получить доступ к заданиям.");
                return;
            }

            // Получаем идентификатор задачи из параметра команды
            int taskId = (int)((Button)sender).CommandParameter;

            // Ищем задачу по идентификатору в коллекции
            DailyTasks task = DataBase.Base.DailyTasks.FirstOrDefault(t => t.Id == taskId);

            if (task != null)
            {
                var userDailyTask = DataBase.Base.UserDailyTasks.FirstOrDefault(udt => udt.DailyTasksID == task.Id && udt.UserId == user.Id);
                if (userDailyTask != null)
                {
                    MessageBox.Show($"Вы уже выполнили задание \"{task.Title}\".");
                }
                else
                {
                    if (task.TaskTypeId == 1 && user.CompletedCountTicketsClosed >= task.TotalCount)
                    {
                        user.Score += task.Score;
                        user.XP += task.XP;
                        user.UserDailyTasks.Add(new UserDailyTasks { DailyTasksID = task.Id, IsCompleted = true });
                        MessageBox.Show($"Вы получили {task.Score} очков и {task.XP} опыта за выполнение задания \"{task.Title}\"");
                        DataBase.Base.SaveChanges();
                    }
                    else if (task.TaskTypeId == 2 && user.CreateCountTickets >= task.TotalCount)
                    {
                        user.Score += task.Score;
                        user.XP += task.XP;
                        user.UserDailyTasks.Add(new UserDailyTasks { DailyTasksID = task.Id, IsCompleted = true });
                        MessageBox.Show($"Вы получили {task.Score} очков и {task.XP} опыта за выполнение задания \"{task.Title}\"");
                        DataBase.Base.SaveChanges();
                    }
                    else
                    {
                        string message = task.TaskTypeId == 1
                            ? "У вас недостаточно выполненных заявок закрытия для выполнения задания"
                            : "У вас недостаточно созданных заявок для выполнения задания";

                        MessageBox.Show($"{message} \"{task.Title}\".");
                    }
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
                    // Удаляем связанные записи из таблицы UserAchievements
                    var tasksToDelete = DataBase.Base.UserDailyTasks.Where(x => x.DailyTasksID == selectedItem.Id);
                    DataBase.Base.UserDailyTasks.RemoveRange(tasksToDelete);

                    // Удаляем элемент из таблицы Achievements
                    DataBase.Base.DailyTasks.Remove(selectedItem);

                    try
                    {
                        // Сохраняем изменения в базе данных
                        DataBase.Base.SaveChanges();

                        // Обновляем содержимое ListView
                        lstDailyTasks.ItemsSource = DataBase.Base.DailyTasks.ToList();

                        // Уведомление об успешном удалении
                        MessageBox.Show($"Задание '{selectedItem.Title}' успешно удалено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        // Обрабатываем возможные ошибки при сохранении изменений
                        MessageBox.Show($"Ошибка при удалении задания: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFilter.SelectedItem != null)
            {
                if (cbFilter.SelectedItem.ToString() == "Все задания")
                {
                    lstDailyTasks.ItemsSource = DataBase.Base.DailyTasks.ToList();
                }
                else if (cbFilter.SelectedItem.ToString() == "Активные задания")
                {
                    var tasks = DataBase.Base.DailyTasks.Where(t => !t.UserDailyTasks.Any(udt => udt.UserId == user.Id && udt.IsCompleted));
                    lstDailyTasks.ItemsSource = tasks.ToList();
                }
                else if (cbFilter.SelectedItem.ToString() == "Выполненные задания")
                {
                    var tasks = DataBase.Base.DailyTasks.Where(t => t.UserDailyTasks.Any(udt => udt.UserId == user.Id && udt.IsCompleted));
                    lstDailyTasks.ItemsSource = tasks.ToList();
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный элемент
            var selectedItem = lstDailyTasks.SelectedItem as DailyTasks;
            if (selectedItem != null)
            {
                // Создаем окно редактирования и передаем в него объект Achievements для редактирования
                Window_AddDailyTasks editWindow = new Window_AddDailyTasks(user, selectedItem);

                // Заполняем поля в окне редактирования текущими значениями достижения
                editWindow.tbTitle.Text = selectedItem.Title;
                editWindow.tbDescription.Text = selectedItem.Description;
                if (selectedItem.Image != null)
                {
                    string imagePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Image\\" + selectedItem.Image;
                    if (File.Exists(imagePath))
                    {
                        BitmapImage img = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                        editWindow.imgDailyTasks.ImageSource = img;
                    }
                }

                // Открываем окно редактирования
                if (editWindow.ShowDialog() == true)
                {
                    // Обновляем объект Achievements в базе данных
                    selectedItem.Title = editWindow.tbTitle.Text;
                    selectedItem.Description = editWindow.tbDescription.Text;
                    if (editWindow.newFilePath != null)
                    {
                        selectedItem.Image = editWindow.newFilePath.Substring(editWindow.newFilePath.LastIndexOf('\\')).Replace("\\", "");
                    }
                    DataBase.Base.SaveChanges();

                    // Выводим сообщение об успешном изменении
                    MessageBox.Show("Достижение успешно изменено!");
                }
            }
        }
    }
}
