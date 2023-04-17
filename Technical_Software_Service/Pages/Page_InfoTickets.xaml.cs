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
    /// Логика взаимодействия для Page_InfoTickets.xaml
    /// </summary>
    public partial class Page_InfoTickets : Page
    {
        Users user;
        Tickets ticket;
        public Page_InfoTickets(Users user, Tickets ticket)
        {
            InitializeComponent();
            this.user = user;
            this.ticket = ticket;
            tbTitle.Inlines.Add(new Run("Наименование заявки: ") { FontWeight = FontWeights.Bold });
            tbTitle.Inlines.Add(new Run($" {ticket.Title}"));
            tbDateOpen.Inlines.Add(new Run("Дата открытия заявки: ") { FontWeight = FontWeights.Bold });
            tbDateOpen.Inlines.Add(new Run($" {string.Format("{0:dd.MM.yyyy}", ticket.OpeningDate)}"));
            tbRequester.Inlines.Add(new Run("Заказчик: ") { FontWeight = FontWeights.Bold });
            tbRequester.Inlines.Add(new Run($" {ticket.Requesters}"));
            tbDescription.Text = ticket.Description;
            tbCategories.Inlines.Add(new Run("Категория: ") { FontWeight = FontWeights.Bold });
            tbCategories.Inlines.Add(new Run($" {ticket.Categories.Kind}"));
            tbImportance.Inlines.Add(new Run("Важность: ") { FontWeight = FontWeights.Bold });
            tbImportance.Inlines.Add(new Run($" {ticket.ImportanceTypes.Kind}"));
            tbStates.Inlines.Add(new Run("Состояние: ") { FontWeight = FontWeights.Bold });
            tbStates.Inlines.Add(new Run($" {ticket.TicketStates.Kind}"));
            tbUsers.Inlines.Add(new Run("Исполнитель: ") { FontWeight = FontWeights.Bold });
            List<HistoryEntries> histories = DataBase.Base.HistoryEntries.Where(x => x.TicketId == ticket.Id).ToList();
            HistoryEntries historyEntries = new HistoryEntries();
            for (int i = 0; i < histories.Count; i++)
            {
                if (i == histories.Count - 1)
                {
                    historyEntries = histories[i];
                }
            }
            Users users = DataBase.Base.Users.FirstOrDefault(x => x.Id == historyEntries.UserId);
            tbUsers.Inlines.Add(new Run($" {users.NameUsers}"));        
            tbDateUpdate.Inlines.Add(new Run("Последнее обновление: ") { FontWeight = FontWeights.Bold });
            tbDateUpdate.Inlines.Add(new Run($" {users.NameUsers + " " + string.Format("{0:dd.MM.yyyy}", ticket.LastUpdate)}"));
            tbSolution.Inlines.Add(new Run("Решение: \n") { FontWeight = FontWeights.Bold });
            tbSolution.Inlines.Add(new Run($" {ticket.Solutions.Content}"));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.MainF.Navigate(new Page_Anything(user));
        }
    }
}
