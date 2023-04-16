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
    /// Логика взаимодействия для Page_Authorization.xaml
    /// </summary>
    public partial class Page_Authorization : Page
    {
        public Page_Authorization()
        {
            InitializeComponent();
            tbLogin.Focus();
            //tbLogin.Text = "APotapin";
            //pbPassword.Password = "admin";
        }

        List<Users> users = new List<Users>();

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            if(checkData(tbLogin.Text, pbPassword.Password))
            {
                string login = tbLogin.Text;
                string password = pbPassword.Password;
                bool isAuth = false;
                users = DataBase.Base.Users.ToList();
                foreach (Users users in users)
                {
                    if (users.UserName == login && users.Password == password)
                    {
                        DataBase.users = users;
                        ClassFrame.MainF.Navigate(new Page_Anything(users)); isAuth = true;
                        break;
                    }
                }
                if (!isAuth)
                {
                    MessageBox.Show("Логин или пароль неверен");
                }
            }            
        }

        public static bool checkData(string a, string b)
        {
            if (a == "" || b == "")
            {
                MessageBox.Show("Обязательные поля не заполнены", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else
            {
                return true;
            }

        }

        private void cbShowPassword_Click(object sender, RoutedEventArgs e) // Показ пароля
        {
            if (cbShowPassword.IsChecked == true)
            {
                tbPassword.Text = pbPassword.Password;

                pbPassword.Visibility = Visibility.Hidden;
                tbPassword.Visibility = Visibility.Visible;
            }
            else
            {
                pbPassword.Visibility = Visibility.Visible;
                tbPassword.Visibility = Visibility.Hidden;
            }
        }
    }
}
