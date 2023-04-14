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
    /// Логика взаимодействия для Page_Achievment.xaml
    /// </summary>
    public partial class Page_Achievment : Page
    {
        Users user;
        Achievements achievm;
        public Page_Achievment(Users user)
        {
            InitializeComponent();
            this.user = user;
            lstAchievment.ItemsSource = DataBase.Base.Achievements.ToList();
            if (user.Roles.Kind == "Администратор")
            {
                btnAdd.Visibility = Visibility.Visible;
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
    }
}
