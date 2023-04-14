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
        public Page_InfoTickets(Users user, Tickets tickets)
        {
            InitializeComponent();
            this.user = user;
            tbTitle.Text = "Наименование заявки: " + tickets.Title;
            tbDateOpen.Text = "Дата открытия заявки: " + string.Format("{0:dd.MM.yyyy}", tickets.OpeningDate);
            tbRequester.Text = "Заказчик: " + tickets.Requesters;
            tbDescription.Text = tickets.Description;
            tbCategories.Text = "Категория: " + tickets.Categories.Kind;
            tbImportance.Text = "Важность: " + tickets.ImportanceTypes.Kind;
            tbStates.Text = "Состояние: " + tickets.TicketStates.Kind;
            tbUsers.Text = "Исполнитель: " + user.NameUsers;
            tbDateUpdate.Text = "Последнее обновление: " + user.LastName + " " + user.FirstName + " " + tickets.LastUpdate;
            tbSolution.Text = tickets.Solutions.Content;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.MainF.Navigate(new Page_Anything(user));
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Window_AddUpdateTickets updateTickets = new Window_AddUpdateTickets(user);
            updateTickets.ShowDialog();
        }
    }
}
