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
            cbStates.SelectedIndex = ticket.TicketStateId - 1;
            cbCategories.SelectedIndex = ticket.CategoryId - 1;
            cbImportance.SelectedIndex = ticket.ImportanceTypeId - 1;

            List<Solutions> solutions = DataBase.Base.Solutions.ToList();
            for (int i = 0; i < solutions.Count; i++)
            {
                cbSolutions.Items.Add(solutions[i].Title);
            }
            cbSolutions.SelectedIndex = (int)ticket.SolutionId - 1;

            //List<Users> users = DataBase.Base.Users.ToList();
            //for (int i = 0; i < users.Count; i++)
            //{
            //    cbUsers.Items.Add(users[i].NameUsers);
            //}
            //cbUsers.SelectedIndex = user.Id - 1;

            cbUsers.SelectedIndex = user.Id - 1;
            tbHeader.Text = "Редатирование заявки";
            btnAdd.Content = "Изменить";

        }
        public Window_AddUpdateTickets(Users user,MimeMessage message)
        {
            InitializeComponent();
            CBUpload();
            this.user = user;
            tbTitle.Text = message.Subject;
            tbRequester.Text = message.From.ToString().Substring(0, message.From.ToString().IndexOf('<')).Trim().Replace("\"","");
            tbDescription.Text = message.TextBody;
            dpOpeningDate.Text = message.Date.ToString();
        }

        /// <summary>
        /// Заполнение ComboBox
        /// </summary>
        public void CBUpload()
        {
            cbStates.ItemsSource = DataBase.Base.TicketStates.ToList();
            cbStates.SelectedValuePath = "Id";
            cbStates.DisplayMemberPath = "Kind";
            cbStates.SelectedIndex = 0;

            cbCategories.ItemsSource = DataBase.Base.Categories.ToList();
            cbCategories.SelectedValuePath = "Id";
            cbCategories.DisplayMemberPath = "Kind";
            cbCategories.SelectedIndex = 0;

            cbImportance.ItemsSource = DataBase.Base.ImportanceTypes.ToList();
            cbImportance.SelectedValuePath = "Id";
            cbImportance.DisplayMemberPath = "Kind";
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
                ticket.CategoryId = Convert.ToInt32(cbCategories.SelectedValue);
                ticket.ImportanceTypeId = Convert.ToInt32(cbImportance.SelectedValue);
                if (cbSolutions.SelectedIndex == 0)
                {
                    ticket.SolutionId = 0;
                }
                else
                {
                    ticket.SolutionId = cbSolutions.SelectedIndex;
                }
                if (dpLastUpdate.SelectedDate == null)
                {
                    dpLastUpdate.SelectedDate = null;
                }
                else
                {
                    ticket.LastUpdate = dpLastUpdate.SelectedDate.Value;
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
                if (cbSolutions.SelectedIndex == 0)
                {
                    ticket.SolutionId = 0;
                }
                else
                {
                    ticket.SolutionId = cbSolutions.SelectedIndex + 1 ;
                }
                if (dpLastUpdate.SelectedDate == null)
                {
                    dpLastUpdate.SelectedDate = null;
                }
                else
                {
                    ticket.LastUpdate = dpLastUpdate.SelectedDate.Value;
                }
                DataBase.Base.Tickets.Add(ticket);
                DataBase.Base.SaveChanges();
                MessageBox.Show("Заявка успешно добавлена!");
                this.Close();
            }


            //Tickets ticket = new Tickets();
            //ticket.Title = tbTitle.Text;
            //ticket.Description = tbDescription.Text;
            //ticket.Requester = tbRequester.Text;
            //ticket.OpeningDate = dpOpeningDate.SelectedDate.Value;
            //ticket.TicketStateId = Convert.ToInt32(cbStates.SelectedValue);
            //ticket.CategoryId = Convert.ToInt32(cbCategories.SelectedValue);
            //ticket.ImportanceTypeId = Convert.ToInt32(cbImportance.SelectedValue);
            //ticket.SolutionId = Convert.ToInt32(cbSolutions.SelectedValue);
            ////ticket.TicketStateId = Convert.ToInt32(cbStates.SelectedValue);  для исполнителя
            //ticket.LastUpdate = dpLastUpdate.SelectedDate.Value;
            //DataBase.Base.Tickets.Add(ticket);

            //HistoryEntries historyEntries = new HistoryEntries(); // Добавление истории заявок
            //historyEntries.UserId = Convert.ToInt32(cbUsers.SelectedValue);
            //historyEntries.TicketId = Convert.ToInt32(ticket.Id);
            //historyEntries.UpdateDescription = ;
            //DataBase.Base.Tickets.Add(historyEntries);

            //Executors executors = new Executors(); // Добавление исполнителей
            //executors.UserId = Convert.ToInt32(cbUsers.SelectedValue);
            //executors.TicketId = ticket.Id;
            //DataBase.Base.Tickets.Add(executors);

            DataBase.Base.SaveChanges();
            MessageBox.Show("Заявка успешно добавлена!");
            this.Close();
        }
    }
}