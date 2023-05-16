using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Page_Achievment.xaml
    /// </summary>
    public partial class Page_Achievment : Page
    {
        Users user;

        public Page_Achievment(Users user )
        {
            InitializeComponent();
            this.user = user;

            tbCompleteTicketProgress.Text = $"Закрыто заявок: {user.CompletedCountTicketsClosed}\nСоздано заявок: {user.CreateCountTickets}";

            lstAchievment.ItemsSource = DataBase.Base.Achievements.ToList();

            // Добавляем элементы в ComboBox
            cbFilter.Items.Add("Все достижения");
            cbFilter.Items.Add("Разблокированные достижения");
            cbFilter.SelectedIndex = 0; // Выбираем первый элемент по умолчанию
            cbFilter.SelectionChanged += cbFilter_SelectionChanged;

            //Отображение кнопок
            if (user.Roles.Kind == "Администратор")
            {
                //btnAdd.Visibility = Visibility.Visible;
                //btnDelete.Visibility = Visibility.Visible;
                btnEdit.Visibility = Visibility.Visible;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.MainF.Navigate(new Page_Anything(user));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window_AddAchievment achievment = new Window_AddAchievment(user);
            achievment.ShowDialog();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFilter = cbFilter.SelectedItem.ToString();

            switch (selectedFilter)
            {
                case "Все достижения":
                    lstAchievment.ItemsSource = DataBase.Base.Achievements.ToList();
                    break;
                case "Разблокированные достижения":
                    var tasks = DataBase.Base.Achievements.Where(a => a.UserAchievements.Any(ua => ua.UserId == user.Id && ua.IsCompleted));
                    lstAchievment.ItemsSource = tasks.ToList();
                    break;
                default:
                    break;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный элемент
            var selectedItem = lstAchievment.SelectedItem as Achievements;
            if (selectedItem != null)
            {
                // Запрашиваем подтверждение удаления
                var result = MessageBox.Show($"Вы уверены, что хотите удалить достижение '{selectedItem.Title}'?", "Удаление достижения", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Удаляем связанные записи из таблицы UserAchievements
                    var tasksToDelete = DataBase.Base.UserAchievements.Where(x => x.AchievementID == selectedItem.Id);
                    DataBase.Base.UserAchievements.RemoveRange(tasksToDelete);

                    // Удаляем элемент из таблицы Achievements
                    DataBase.Base.Achievements.Remove(selectedItem);

                    try
                    {
                        // Сохраняем изменения в базе данных
                        DataBase.Base.SaveChanges();

                        // Обновляем содержимое ListView
                        lstAchievment.ItemsSource = DataBase.Base.Achievements.ToList();

                        // Уведомление об успешном удалении
                        MessageBox.Show($"Достижение '{selectedItem.Title}' успешно удалено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        // Обрабатываем возможные ошибки при сохранении изменений
                        MessageBox.Show($"Ошибка при удалении достижения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void bFinishAchievment_Click(object sender, RoutedEventArgs e)
        {
            // вызов методов для проверки и разблокировки достижений при завершении задачи
            AchievementsManager.CheckClosedTicketsAchievements(user);
            AchievementsManager.CheckCreatedTicketsAchievements(user);
            AchievementsManager.CheckLevelAchievements(user);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный элемент
            var selectedItem = lstAchievment.SelectedItem as Achievements;
            if (selectedItem != null)
            {
                // Создаем окно редактирования и передаем в него объект Achievements для редактирования
                Window_AddAchievment editWindow = new Window_AddAchievment(user, selectedItem);

                // Заполняем поля в окне редактирования текущими значениями достижения
                editWindow.tbTitle.Text = selectedItem.Title;
                editWindow.tbDescription.Text = selectedItem.Description;
                if (selectedItem.Image != null)
                {
                    string imagePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Image\\" + selectedItem.Image;
                    if (File.Exists(imagePath))
                    {
                        BitmapImage img = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                        editWindow.imgAchievment.ImageSource = img;
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
