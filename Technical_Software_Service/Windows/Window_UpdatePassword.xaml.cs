using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Technical_Software_Service
{
    /// <summary>
    /// Логика взаимодействия для Window_UpdatePassword.xaml
    /// </summary>
    public partial class Window_UpdatePassword : Window
    {
        Users user;
        public Window_UpdatePassword(Users user)
        {
            InitializeComponent();
            this.user = user;
            tbLogin.Text = user.UserName;
        }

        private void cbShowNewPassword_Click(object sender, RoutedEventArgs e) // Показ нового пароля
        {
            if (cbShowNewPassword.IsChecked == true)
            {
                tbNewPassword.Text = pbNewPassword.Password;

                pbNewPassword.Visibility = Visibility.Hidden;
                tbNewPassword.Visibility = Visibility.Visible;
            }
            else
            {
                pbNewPassword.Visibility = Visibility.Visible;
                tbNewPassword.Visibility = Visibility.Hidden;
            }
        }
        private void cbShowOldPassword_Click(object sender, RoutedEventArgs e) // Показ старого пароля
        {
            if (cbShowOldPassword.IsChecked == true)
            {
                tbOldPassword.Text = pbOldPassword.Password;

                pbOldPassword.Visibility = Visibility.Hidden;
                tbOldPassword.Visibility = Visibility.Visible;
            }
            else
            {
                pbOldPassword.Visibility = Visibility.Visible;
                tbOldPassword.Visibility = Visibility.Hidden;
            }
        }
        private void cbPasswordShow_Click(object sender, RoutedEventArgs e) // Показ при повторе нового пароля
        {
            if (cbPasswordShow.IsChecked == true)
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
        /// <summary>
        /// Метод для проверки пароля
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsPass(string password)
        {          
            Regex r = new Regex("(?=.*[A-Z])");
            Regex r1 = new Regex("[a-z].*[a-z].*[a-z]");
            Regex r2 = new Regex("\\d.*\\d");
            if (r.IsMatch(password) == true)
            {
                if (r1.IsMatch(password) == true)
                {
                    if (r2.IsMatch(password) == true)
                    {
                        if (password.Length >= 8)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Общая длина пароля не менее 8 символов!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Пароль должен содержать не менее 2 цифры!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Пароль должен содержать не менее 3 строчных латинских символов!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Пароль должен содержать не менее 1 заглавного латинского символа!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }
        /// <summary>
        /// Метод для проверки совпадения паролей
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public static bool CoincidencePass(string pass, string newPass)
        {
            if (pass == newPass)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Пароли не совпадают");
                return false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string password = pbOldPassword.Password;
                Users user = DataBase.Base.Users.FirstOrDefault(x => x.UserName == tbLogin.Text && x.Password == password);
                if (user == null)
                {
                    MessageBox.Show("Введенный пароль неправильный", "Смена пароля", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (IsPass(pbNewPassword.Password))
                    {
                        if (CoincidencePass(pbNewPassword.Password, pbPassword.Password))
                        {
                            user.Password = pbPassword.Password;
                            user.UserName = tbLogin.Text;
                            DataBase.Base.SaveChanges();
                            MessageBox.Show("Пароль успешно изменен!");
                            this.Close();
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Что-то не так", "Ошибка");
            }            
        }        
    }
}
