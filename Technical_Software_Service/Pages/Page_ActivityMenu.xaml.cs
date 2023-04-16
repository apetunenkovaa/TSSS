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
using MailKit;
using MimeKit;
using MailKit.Net.Imap;
using MailKit.Security;
using Technical_Software_Service;
using System.Reflection;

namespace Technical_Software_Service
{
    /// <summary>
    /// Логика взаимодействия для Page_Anything.xaml
    /// </summary>
    public partial class Page_Anything : Page
    {
        Users user;
        public Page_Anything(Users user)
        {
            InitializeComponent();
            this.user = user;
            UpdateList();
            Filter();
            ListAnything.ItemsSource = DataBase.Base.Tickets.ToList();
            ListHistory.ItemsSource = DataBase.Base.HistoryEntries.ToList();
            dgUsers.ItemsSource = DataBase.Base.Users.ToList();
            dgUsersRating.ItemsSource = DataBase.Base.Users.ToList(); // Страница Рейтинг пользователей

            tbUserName.Text = tbUserName.Text + user.LastName + " " + user.FirstName + " " + user.MiddleName;
            tbUserRole.Text = tbUserRole.Text + user.Roles.Kind;
            tbUserPost.Text = tbUserPost.Text + user.Positions.Kind;
            tbUserScore.Text = tbUserScore.Text + user.Score;
            if (user.Photo != null)
            {
                string path = Environment.CurrentDirectory.Replace("bin\\Debug", "Image/");
                BitmapImage img = new BitmapImage(new Uri(path+user.Photo, UriKind.RelativeOrAbsolute));
                PhotoUser.ImageSource = img;

            }
            // Отображение только для администратора
            if (user.Roles.Kind == "Администратор")
            {
                tcUsers.Visibility = Visibility.Visible;
                titNotifications.Visibility = Visibility.Visible;
            }
            cbFilter.SelectedIndex = 0; // Фильтр для заявок
            cboxFilter.SelectedIndex = 0; // Фильтр для истории заявок
        }

        /// <summary>
        ///  Поиск и фильтрация страницы Заявки
        /// </summary>
        public void Filter()
        {
            List<Tickets> listFilter = DataBase.Base.Tickets.ToList();
            // Поиск
            if (!string.IsNullOrWhiteSpace(tbSearch.Text))
            {
                listFilter = listFilter.Where(x => x.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList(); // Поиск по наименованию
            }

            // Фильтрация
            switch (cbFilter.SelectedIndex)
            {
                case 1:
                    listFilter = listFilter.Where(z=>z.ImportanceTypeId == 1).ToList();
                    break;
                case 2:
                    listFilter = listFilter.Where(z => z.ImportanceTypeId == 2).ToList();
                    break;
                case 3:
                    listFilter = listFilter.Where(z => z.ImportanceTypeId == 3).ToList();
                    break;
                case 4:
                    listFilter = listFilter.Where(z => z.ImportanceTypeId == 4).ToList();
                    break;
            }

            tbCount.Text = listFilter.Count.ToString() + " из " + DataBase.Base.Tickets.ToList().Count.ToString(); //количество записей

            ListAnything.ItemsSource = listFilter;
            if (listFilter.Count == 0)
            {
                MessageBox.Show("Нет записей");
            }
        }
        /// <summary>
        ///  Поиск и фильтрация страницы История заявок
        /// </summary>
        public void HistoryFilter()
        {
            List<HistoryEntries> listFilterhistory = DataBase.Base.HistoryEntries.ToList();
            // Поиск
            if (!string.IsNullOrWhiteSpace(tboxSearch.Text))
            {
                listFilterhistory = listFilterhistory.Where(x => x.Tickets.Title.ToLower().Contains(tboxSearch.Text.ToLower())).ToList(); // Поиск по наименованию заявки
            }
            else if (!string.IsNullOrWhiteSpace(tboxSearch.Text))
            {
                listFilterhistory = listFilterhistory.Where(x => x.Users.LastName.ToLower().Contains(tboxSearch.Text.ToLower())).ToList(); // Поиск по фамилии пользователя
            }

            // Фильтрация
            if (cboxFilter.SelectedIndex > 0)
            {
                List<Tickets> tickets = DataBase.Base.Tickets.Where(z => z.TicketStateId == cboxFilter.SelectedIndex).ToList();
                List<HistoryEntries> histories = new List<HistoryEntries>();
                for(int i = 0; i < tickets.Count; i++)
                {
                    List<HistoryEntries> NewHistories = new List<HistoryEntries>();
                    NewHistories = listFilterhistory.Where(x=>x.TicketId==tickets[i].Id).ToList();
                    if (NewHistories.Count > 0)
                    {
                        for(int j = 0; j < NewHistories.Count; j++)
                        {
                            histories.Add(NewHistories[j]);
                        }

                    }
                }
                listFilterhistory = histories;
            }
            ListHistory.ItemsSource = listFilterhistory;
            if (listFilterhistory.Count == 0)
            {
                MessageBox.Show("Нет записей");
            }
        }
        /// <summary>
        /// Поиск на странице Пользователи
        /// </summary>
        public void UsersFilter()
        {
            List<Users> listFilterUsers = DataBase.Base.Users.ToList();
            // Поиск
            if (!string.IsNullOrWhiteSpace(tbSearchUsers.Text))
            {
                listFilterUsers = listFilterUsers.Where(x => x.LastName.ToLower().Contains(tbSearchUsers.Text.ToLower())).ToList(); // Поиск по фамилии
            }
            else if (!string.IsNullOrWhiteSpace(tbSearchUsers.Text))
            {
                listFilterUsers = listFilterUsers.Where(x => x.FirstName.ToLower().Contains(tbSearchUsers.Text.ToLower())).ToList(); // Поиск по имени
            }

            dgUsers.ItemsSource = listFilterUsers;
            if (listFilterUsers.Count == 0)
            {
                MessageBox.Show("Нет записей");
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }
        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
        private void tboxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            HistoryFilter();
        }
        private void cboxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HistoryFilter();
        }
        private void tbSearchUsers_TextChanged(object sender, TextChangedEventArgs e)
        {
            UsersFilter();
        }
        private void btnDailytasks_Click(object sender, RoutedEventArgs e) // Переход к ежедневным задачам
        {
            ClassFrame.MainF.Navigate(new Page_DailyTasks(user));
        }
        private void bntExit_Click(object sender, RoutedEventArgs e) //  Выход из учетной записи
        {
            ClassFrame.MainF.Navigate(new Page_Authorization());
        }
        private void btnAchievement_Click(object sender, RoutedEventArgs e) // Переход к достижениям
        {
            ClassFrame.MainF.Navigate(new Page_Achievment(user));
        }
        private void btnAddUser_Click(object sender, RoutedEventArgs e) // Добавление пользователя
        {
            Window_Users users = new Window_Users();
            users.ShowDialog();
            dgUsers.ItemsSource = DataBase.Base.Users.ToList();
        }

        private void btnUpdateAccount_Click(object sender, RoutedEventArgs e) //Редактирование данных
        {
            Window_UpdatePassword updatePassword = new Window_UpdatePassword(user);
            updatePassword.ShowDialog();
        }

        private void dgUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Users user = new Users();
                foreach (Users users in dgUsers.SelectedItems)
                {
                    user = users;
                }
                if (user == null)
                {
                    return;
                }
                else
                {
                    Window_Users _users = new Window_Users(user);
                    _users.ShowDialog();
                    dgUsers.ItemsSource = DataBase.Base.Users.ToList();
                }

            }
            catch
            {
                MessageBox.Show("Нажмите на 1 объект!!!");
            }
        }
        //ZdK9zqCN2hJ24mkkxxLF
        List<ClassMessage> messages = new List<ClassMessage>();
        //Обновление листа
        private void UpdateList()
        {
            messages.Clear();
            using (ImapClient client = new ImapClient())
            {
                client.Connect("imap.mail.ru", 993, true);
                client.Authenticate("helpdesk_chit@mail.ru", "ZdK9zqCN2hJ24mkkxxLF");
                IMailFolder inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                for (int i = 0; i < inbox.Count; i++)
                {
                    MimeMessage messagees = inbox.GetMessage(i);
                    ClassMessage cm = new ClassMessage();

                    cm.message = messagees;
                    cm.id = i;
                    cm.subject = messagees.Subject.ToString();
                    cm.from = messagees.From.ToString();
                    messages.Add(cm);

                }
            }
            notificationsLV.ItemsSource = Enumerable.Reverse(messages);
            notificationsLV.SelectedValuePath = "id";

        }

        private void notificationsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = Convert.ToInt32(notificationsLV.SelectedValue);
            MimeMessage mime = messages.FirstOrDefault(x => x.id == id).message;

            tbSubject.Text = mime.Subject.ToString();
            tbSender.Text = mime.From.ToString();
            tbBody.Text = mime.TextBody.ToString();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e) // Добавление заявки
        {
            int id = Convert.ToInt32(notificationsLV.SelectedValue);
            MimeMessage mime = messages.FirstOrDefault(x => x.id == id).message;
            Window_AddUpdateTickets tickets = new Window_AddUpdateTickets(user, mime);
            tickets.ShowDialog();

            ClassFrame.MainF.Navigate(new Page_Anything(user));
            //tickets.Closing += (obj, args) =>
            //{
            //    ListAnything.ItemsSource = DataBase.Base.Tickets.ToList();
            //};
            
        }

        private void btnUpdate_Click_1(object sender, RoutedEventArgs e) // Обновление листа 
        {
            UpdateList();
        }

        private void ListAnything_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Tickets ticket = new Tickets();
                foreach (Tickets tickets in ListAnything.SelectedItems)
                {
                    ticket = tickets;
                }
                if (ticket == null)
                {
                    return;
                }
                else
                {
                    ClassFrame.MainF.Navigate(new Page_InfoTickets(user, ticket));
                }

            }
            catch
            {
                MessageBox.Show("Нажмите на 1 объект!!!");
            }
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e) // Удаление пользователя
        {
            //try
            //{
            if (dgUsers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Не выбран ни один пользователь!");
            }
            else
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить данного пользователя?", "Системное сообщение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //Users users = DataBase.Base.Users.FirstOrDefault(x => x.Id == index);
                    List<HistoryEntries> historyEntries = DataBase.Base.HistoryEntries.Where(x => x.UserId == user.Id).ToList();
                    if (historyEntries.Count == 0) // Если пользователь остутствует в истории заявок, то пользователя можно удалить
                    {
                        DataBase.Base.Users.Remove(user);
                        DataBase.Base.SaveChanges();
                        dgUsers.ItemsSource = DataBase.Base.Users.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Данный пользователь не может быть удален, так как используется в историях заявок");
                    }

                }
            }              
            //}
            //catch
            //{
            //    MessageBox.Show("При удаление товара возникла ошибка");
            //}                        
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e) // Редактирование заявки
        {
            Button btn = (Button)sender;
            int index = Convert.ToInt32(btn.Uid);
            Tickets ticket = DataBase.Base.Tickets.FirstOrDefault(z => z.Id == index);
            Window_AddUpdateTickets updateTickets = new Window_AddUpdateTickets(user, ticket);
            updateTickets.ShowDialog();
            ClassFrame.MainF.Navigate(new Page_Anything(user));
        }
    }
}

