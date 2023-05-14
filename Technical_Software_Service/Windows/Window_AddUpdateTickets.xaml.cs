using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;
using MimeKit;
namespace Technical_Software_Service
{
    /// <summary>
    /// Логика взаимодействия для Window_AddUpdateTickets.xaml
    /// </summary>
    public partial class Window_AddUpdateTickets : Window
    {
        
        Users user;
        bool flag = false;
        Tickets ticket;
        public Window_AddUpdateTickets(Users user)
        {
            InitializeComponent();
            CBUpload();
            this.user = user;
        }
        public Window_AddUpdateTickets(Users user, Tickets ticket) // Конструктор для редактирования
        {
            InitializeComponent();
            this.user = user;
            this.ticket = ticket;
            flag = true;
            tbTitle.Text = ticket.Title;
            tbRequester.Text = ticket.Requester;
            dpOpeningDate.SelectedDate = ticket.OpeningDate;
            tbDescription.Text = ticket.Description;
            dpLastUpdate.SelectedDate = ticket.LastUpdate;

            List<TicketStates> ticketStates = DataBase.Base.TicketStates.ToList();
            for (var i = 0; i < ticketStates.Count; i++)
            {
                cbStates.Items.Add(ticketStates[i].Kind);
            }
            cbStates.SelectedIndex = ticket.TicketStateId - 1;

            List<Categories> category = DataBase.Base.Categories.ToList();
            for (var i = 0; i < category.Count; i++)
            {
                cbCategories.Items.Add(category[i].Kind);
            }
            cbCategories.SelectedIndex = ticket.CategoryId - 1;

            List<ImportanceTypes> types = DataBase.Base.ImportanceTypes.ToList();
            for (var i = 0; i < types.Count; i++)
            {
                cbImportance.Items.Add(types[i].Kind);
            }
            cbImportance.SelectedIndex = ticket.ImportanceTypeId - 1;


            List<Solutions> solutions = DataBase.Base.Solutions.ToList();
            cbSolutions.Items.Add("не выбрано");
            for (int i = 0; i < solutions.Count; i++)
            {
                cbSolutions.Items.Add(solutions[i].Title);
            }
            if (ticket.SolutionId == null)
            {
                cbSolutions.SelectedIndex = 0;
            }
            else
            {
                cbSolutions.SelectedIndex = (int)ticket.SolutionId;
            }


            List<Users> users = DataBase.Base.Users.ToList();
            cbUsers.Items.Add("не выбрано");
            for (int i = 0; i < users.Count; i++)
            {
                cbUsers.Items.Add(users[i].NameUsers);
            }
            List<HistoryEntries> histories = DataBase.Base.HistoryEntries.Where(x => x.TicketId == ticket.Id).ToList();
            HistoryEntries historyEntries = new HistoryEntries();
            for (int i = 0; i < histories.Count; i++)
            {
                if (i == histories.Count - 1)
                {
                    historyEntries = histories[i];
                }
            }
            if (historyEntries == null)
            {
                cbUsers.SelectedIndex = 0;
            }
            else
            {
                cbUsers.SelectedIndex = historyEntries.UserId;
            }

            tbHeader.Text = "Редатирование заявки";
            btnAdd.Content = "Изменить";
            spUpdateTickets.Visibility = Visibility.Visible;

        }
        public Window_AddUpdateTickets(Users user, MimeMessage message)
        {
            InitializeComponent();
            CBUpload();
            this.user = user;
            tbTitle.Text = message.Subject;
            tbRequester.Text = message.From.ToString().Substring(0, message.From.ToString().IndexOf('<')).Trim().Replace("\"", "");
            tbDescription.Text = message.TextBody;
            dpOpeningDate.Text = message.Date.ToString();
        }

        /// <summary>
        /// Заполнение ComboBox
        /// </summary>
        public void CBUpload()
        {
            List<TicketStates> states = DataBase.Base.TicketStates.ToList();
            cbStates.Items.Add("не выбрано");
            for (var i = 0; i < states.Count; i++)
            {
                cbStates.Items.Add(states[i].Kind);
            }
            cbStates.SelectedIndex = 0;

            List<Categories> category = DataBase.Base.Categories.ToList();
            cbCategories.Items.Add("не выбрано");
            for (var i = 0; i < category.Count; i++)
            {
                cbCategories.Items.Add(category[i].Kind);
            }
            cbCategories.SelectedIndex = 0;

            List<ImportanceTypes> importanceTypes = DataBase.Base.ImportanceTypes.ToList();
            cbImportance.Items.Add("не выбрано");
            for (var i = 0; i < importanceTypes.Count; i++)
            {
                cbImportance.Items.Add(importanceTypes[i].Kind);
            }
            cbImportance.SelectedIndex = 0;

            List<Solutions> solutions = DataBase.Base.Solutions.ToList();
            cbSolutions.Items.Add("не выбрано");
            for (var i = 0; i < solutions.Count; i++)
            {
                cbSolutions.Items.Add(solutions[i].Title);
            }
            cbSolutions.SelectedIndex = 0;

            List<Users> users = DataBase.Base.Users.ToList();
            cbUsers.Items.Add("не выбрано");
            for (var i = 0; i < users.Count; i++)
            {
                cbUsers.Items.Add(users[i].NameUsers);
            }
            cbUsers.SelectedIndex = 0;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.MainF.Navigate(new Page_Anything(user));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (flag == false)
            {
                if (tbTitle.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Поле наименование должно быть заполнено!");
                    return;
                }
                if (tbRequester.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Поле заказчик должно быть заполнено!");
                    return;
                }
                if (dpOpeningDate.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Поле дата открытия должно быть заполнено!");
                    return;
                }
                if (cbStates.Text == "")
                {
                    MessageBox.Show("Поле состояние должно быть выбрано из списка!");
                    return;
                }
                if (cbCategories.Text == "")
                {
                    MessageBox.Show("Поле категория должно быть выбрано из списка!");
                    return;
                }
                if (cbImportance.Text == "")
                {
                    MessageBox.Show("Поле важность должно быть выбрано из списка!");
                    return;
                }
                Tickets ticket = new Tickets();
                ticket.Title = tbTitle.Text;
                if (tbDescription.Text == null)
                {
                    ticket.Description = null;
                }
                else
                {
                    ticket.Description = tbDescription.Text;
                }
                ticket.Requester = tbRequester.Text;
                ticket.OpeningDate = dpOpeningDate.SelectedDate.Value;
                ticket.TicketStateId = cbStates.SelectedIndex;
                ticket.CategoryId = cbCategories.SelectedIndex;
                ticket.ImportanceTypeId = cbImportance.SelectedIndex;
                ticket.SolutionId = null;
                if (dpLastUpdate.SelectedDate == null)
                {
                    dpLastUpdate.SelectedDate = null;
                }
                else
                {
                    ticket.LastUpdate = dpLastUpdate.SelectedDate.Value;
                }

                //Начисление очков пользователю и уведомление
                // Получаем список состояний заявок из базы данных
                List<TicketStates> ticketStates = DataBase.Base.TicketStates.ToList();

                        // Отображаем MessageBox для подтверждения закрытия заявки
                        if (MessageBox.Show("Вы уверены, что хотите добавить эту заявку?", "Добавление заявки", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            // Начисляем очки пользователю за закрытие заявки
                            int score = 0;
                            if (ticket.ImportanceTypeId != null)
                            {
                                switch (ticket.ImportanceTypeId)
                                {
                                    case 1:
                                        score = 10;
                                MessageBox.Show($"Вам начислено {score} очков за добавление заявки");
                                break;
                                    case 2:
                                        score = 15;
                                MessageBox.Show($"Вам начислено {score} очков за добавление заявки");
                                break;
                                    case 3:
                                        score = 5;
                                MessageBox.Show($"Вам начислено {score} очков за добавление заявки");
                                break;
                                    case 4:
                                        score = 1;
                                MessageBox.Show($"Вам начислено {score} очко за добавление заявки");
                                break;
                                    default:
                                        break;
                                }
                            }
                            user.Score += score;

                            //Увеличение счетчик CompletedCount для отслеживания числа закрытых заявок.
                            user.CreateCountTickets++;
                            Debug.WriteLine(user.CreateCountTickets);

                            // Сохраняем изменения в базе данных
                            DataBase.Base.SaveChanges();
                        }

                DataBase.Base.Tickets.Add(ticket);
                DataBase.Base.SaveChanges();
                MessageBox.Show("Заявка успешно добавлена!");
                this.Close();
            }
            else
            {
                if (tbTitle.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Поле наименование должно быть заполнено!");
                    return;
                }
                if (tbRequester.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Поле заказчик должно быть заполнено!");
                    return;
                }
                if (dpOpeningDate.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Поле дата открытия должно быть заполнено!");
                    return;
                }
                if (cbCategories.Text == "")
                {
                    MessageBox.Show("Поле категория должно быть выбрано из списка!");
                    return;
                }
                if (cbCategories.Text == "")
                {
                    MessageBox.Show("Поле категория должно быть выбрано из списка!");
                    return;
                }
                if (cbImportance.Text == "")
                {
                    MessageBox.Show("Поле важность должно быть выбрано из списка!");
                    return;
                }
                ticket.Title = tbTitle.Text;
                if (tbDescription.Text == null)
                {
                    ticket.Description = null;
                }
                else
                {
                    ticket.Description = tbDescription.Text;
                }
                ticket.Requester = tbRequester.Text;
                ticket.OpeningDate = dpOpeningDate.SelectedDate.Value;
                ticket.TicketStateId = cbStates.SelectedIndex + 1;
                ticket.CategoryId = cbCategories.SelectedIndex + 1;
                ticket.ImportanceTypeId = cbImportance.SelectedIndex + 1;
                if (cbSolutions.SelectedIndex != 0)
                {
                    ticket.SolutionId = cbSolutions.SelectedIndex;
                }
                else
                {
                    MessageBox.Show("Заполните поле!");
                }
                if (dpLastUpdate.SelectedDate == null)
                {
                    dpLastUpdate.SelectedDate = null;
                }
                else
                {
                    ticket.LastUpdate = dpLastUpdate.SelectedDate.Value;
                }

                HistoryEntries historyEntries = new HistoryEntries(); // Добавление истории заявок
                historyEntries.UserId = cbUsers.SelectedIndex;
                historyEntries.TicketId = Convert.ToInt32(ticket.Id);
                historyEntries.UpdateDescription = null;
                DataBase.Base.HistoryEntries.Add(historyEntries);


                //Начисление очков пользователю и уведомление
                // Получаем список состояний заявок из базы данных
                List<TicketStates> ticketStates = DataBase.Base.TicketStates.ToList();

                // Проверяем, что в ComboBox был выбран элемент
                if (cbStates.SelectedIndex >= 0)
                {
                    // Получаем выбранный элемент ComboBox
                    var selectedTicket = ticketStates[cbStates.SelectedIndex];

                    // Проверяем, что выбранный элемент ComboBox имеет Id = 2
                    if (selectedTicket.Id == 2)
                    {
                        // Отображаем MessageBox для подтверждения закрытия заявки
                        if (MessageBox.Show("Вы уверены, что хотите закрыть эту заявку?", "Закрытие заявки", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            // Начисляем очки пользователю за закрытие заявки
                            int score = 0;
                            if (ticket.ImportanceTypeId != null)
                            {
                                switch (ticket.ImportanceTypeId)
                                {
                                    case 1:
                                        score = 10;
                                        MessageBox.Show($"Вам начислено {score} очков за закрытие заявки");
                                        break;
                                    case 2:
                                        score = 15;
                                        MessageBox.Show($"Вам начислено {score} очков за закрытие заявки");
                                        break;
                                    case 3:
                                        score = 5;
                                        MessageBox.Show($"Вам начислено {score} очков за закрытие заявки");
                                        break;
                                    case 4:
                                        score = 1;
                                        MessageBox.Show($"Вам начислено {score} очко за закрытие заявки");
                                        break;
                                    default:
                                        break;
                                }
                            }
                            user.Score += score;

                            //Увеличение счетчик CompletedCount для отслеживания числа закрытых заявок.
                            user.CompletedCountTicketsClosed++;
                            Debug.WriteLine(user.CompletedCountTicketsClosed);

                            // Сохраняем изменения в базе данных
                            DataBase.Base.SaveChanges();
                        }
                    }
                }

                Executors executors = new Executors(); // Добавление исполнителей
                executors.UserId = cbUsers.SelectedIndex;
                executors.TicketId = ticket.Id;
                DataBase.Base.Executors.Add(executors);
                DataBase.Base.SaveChanges();
                MessageBox.Show("Заявка успешно изменена!");
                this.Close();
            }
        }
    }
}