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
        Tickets tickets;
        public Page_InfoTickets(Users user, Tickets tickets)
        {
            InitializeComponent();
            this.user = user;
            this.tickets = tickets;
            tbTitle.Inlines.Add(new Run("Наименование заявки: ") { FontWeight = FontWeights.Bold });
            tbTitle.Inlines.Add(new Run($" {tickets.Title}"));
            tbDateOpen.Inlines.Add(new Run("Дата открытия заявки: ") { FontWeight = FontWeights.Bold });
            tbDateOpen.Inlines.Add(new Run($" {string.Format("{0:dd.MM.yyyy}", tickets.OpeningDate)}"));
            tbRequester.Inlines.Add(new Run("Заказчик: ") { FontWeight = FontWeights.Bold });
            tbRequester.Inlines.Add(new Run($" {tickets.Requesters}"));
            tbDescription.Text = tickets.Description;
            tbCategories.Inlines.Add(new Run("Категория: ") { FontWeight = FontWeights.Bold });
            tbCategories.Inlines.Add(new Run($" {tickets.Categories.Kind}"));
            tbImportance.Inlines.Add(new Run("Важность: ") { FontWeight = FontWeights.Bold });
            tbImportance.Inlines.Add(new Run($" {tickets.ImportanceTypes.Kind}"));
            tbStates.Inlines.Add(new Run("Состояние: ") { FontWeight = FontWeights.Bold });
            tbStates.Inlines.Add(new Run($" {tickets.TicketStates.Kind}"));
            tbUsers.Inlines.Add(new Run("Исполнитель: ") { FontWeight = FontWeights.Bold });
            tbUsers.Inlines.Add(new Run($" {user.NameUsers}"));
            tbDateUpdate.Inlines.Add(new Run("Последнее обновление: ") { FontWeight = FontWeights.Bold });
            tbDateUpdate.Inlines.Add(new Run($" {user.LastName + " " + user.FirstName + " " + tickets.LastUpdate}"));
            tbTitleSolution.Inlines.Add(new Run("Наименование решения: ") { FontWeight = FontWeights.Bold });
            tbTitleSolution.Inlines.Add(new Run($" {tickets.Solutions.Title}"));
            tbSolution.Text = tickets.Solutions.Content;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.MainF.Navigate(new Page_Anything(user));
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Window_AddUpdateTickets updateTickets = new Window_AddUpdateTickets(user, tickets);
            updateTickets.ShowDialog();
        }
    }
}
