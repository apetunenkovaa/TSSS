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
        Tickets tickets;
        public Window_AddUpdateTickets(Users user)
        {
            InitializeComponent();
            CBUpload();
            this.user = user;            
        }
        public Window_AddUpdateTickets(Users user, Tickets tickets)
        {
            InitializeComponent();
            CBUpload();
            this.user = user;
            this.tickets = tickets;
            flag = true;
            tbTitle.Text = tickets.Title;      
            tbRequester.Text = tickets.Requester;
            dpOpeningDate.SelectedDate = tickets.OpeningDate;
            tbDescription.Text = tickets.Description;
            dpLastUpdate.SelectedDate = tickets.LastUpdate;
            cbCategories.SelectedIndex = tickets .CategoryId - 1;
            cbImportance.SelectedIndex = tickets.ImportanceTypeId - 1;
            cbStates.SelectedIndex = tickets.TicketStateId - 1;  
            cbSolutions.SelectedIndex = tickets.SolutionId - 1;
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

            cbSolutions.ItemsSource = DataBase.Base.Solutions.ToList();
            cbSolutions.SelectedValuePath = "Id";
            cbSolutions.DisplayMemberPath = "Title";
            cbSolutions.SelectedIndex = 0;

            cbUsers.ItemsSource = DataBase.Base.Users.ToList();
            cbUsers.SelectedValuePath = "Id";
            cbUsers.DisplayMemberPath = "NameUsers";
            cbUsers.SelectedIndex = 0;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.MainF.Navigate(new Page_Anything(user));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Tickets ticket = new Tickets();
            ticket.Title = tbTitle.Text;
            ticket.Description = tbDescription.Text;
            ticket.Requester = tbRequester.Text;
            ticket.OpeningDate = dpOpeningDate.SelectedDate.Value;
            ticket.TicketStateId = Convert.ToInt32(cbStates.SelectedValue);
            ticket.CategoryId = Convert.ToInt32(cbCategories.SelectedValue);
            ticket.ImportanceTypeId = Convert.ToInt32(cbImportance.SelectedValue);
            ticket.SolutionId = Convert.ToInt32(cbSolutions.SelectedValue);
            //ticket.TicketStateId = Convert.ToInt32(cbStates.SelectedValue);  для исполнителя
            ticket.LastUpdate = dpLastUpdate.SelectedDate.Value;
            DataBase.Base.Tickets.Add(ticket);

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