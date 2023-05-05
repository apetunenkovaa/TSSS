using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Technical_Software_Service
{
    /// <summary>
    /// Логика взаимодействия для Window_Users.xaml
    /// </summary>
    public partial class Window_Users : Window
    {
        Users user;
        bool flag = false;
        string NewPath = null; // Путь к картинке
        public Window_Users()
        {
            InitializeComponent();
            listFild();
        }
        public Window_Users(Users user)
        {
            InitializeComponent();
            this.user = user;
            flag = true;

            List<UserStates> userStates = DataBase.Base.UserStates.ToList();
            for (var i = 0; i < userStates.Count; i++)
            {
                cbUserStates.Items.Add(userStates[i].Kind);
            }
            cbUserStates.SelectedIndex = user.UserStateId - 1;

            List<Positions> position = DataBase.Base.Positions.ToList();
            for (var i = 0; i < position.Count; i++)
            {
                cbPosition.Items.Add(position[i].Kind);
            }
            cbPosition.SelectedIndex = user.PositionId - 1;

            tbLastName.Text = user.LastName;
            tbFirstName.Text = user.FirstName;
            tbMiddleName.Text = user.MiddleName;
            tbUserName.Text = user.UserName;
            pbPassword.Password = user.Password;
            tbEmail.Text = user.Email;
            if (user.Photo != null)
            {
                string path = Environment.CurrentDirectory.Replace("bin\\Debug", "Image/");
                BitmapImage img = new BitmapImage(new Uri(path + user.Photo, UriKind.RelativeOrAbsolute));
                imgUser.ImageSource = img;
            }
            tbPhone.Text = user.PhoneNumber;
            cbVisibility.IsChecked = user.Visible;
            tbScore.Text = Convert.ToString(user.Score);
            tbLevel.Text = Convert.ToString(user.Level);
            tbXP.Text = Convert.ToString(user.XP);
            tbHeader.Text = "Изменение данных пользователя";
            btnSave.Content = "Сохранить";
            btnAddPhoto.Content = "Изменить фото";
        }

        /// <summary>
        /// Заполнение ComboBox
        /// </summary>
        public void listFild()
        {

            List<UserStates> userStates = DataBase.Base.UserStates.ToList();
            cbUserStates.Items.Add("не выбрано");
            for (var i = 0; i < userStates.Count; i++)
            {
                cbUserStates.Items.Add(userStates[i].Kind);
            }
            cbUserStates.SelectedIndex = 0;

            // ComboBox с должностью
            List<Positions> position = DataBase.Base.Positions.ToList();
            cbPosition.Items.Add("не выбрана должность");
            for (var i = 0; i < position.Count; i++)
            {
                cbPosition.Items.Add(position[i].Kind);
            }
            cbPosition.Items.Add("Добавить должность");
            cbPosition.SelectedIndex = 0;
        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();
            string path = OFD.FileName;
            if (path != null)
            {
                NewPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Image\\" + System.IO.Path.GetFileName(path); // Путь куда копировать файл
                if (!File.Exists(NewPath)) // Проверка наличия картинки в папке
                {
                    File.Copy(path, NewPath);
                }
                else
                {
                    MessageBox.Show("Такая картинка уже есть! Добавлено старое фото");
                }
                BitmapImage img = new BitmapImage(new Uri(NewPath, UriKind.RelativeOrAbsolute));
                imgUser.ImageSource = img;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Проверка на правильность ввода почты
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsCheckEmail(string email)
        {
            Regex RegexEmail = new Regex("^\\S+@\\S+\\.\\S+$"); // Регулярное выражение для проверки электронной почты
            if (RegexEmail.IsMatch(email) == true)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Введите электронную почту корректно", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (flag == false)
                {
                    if (tbLastName.Text.Replace(" ", "") == "")
                    {
                        MessageBox.Show("Поле фамилия должно быть заполнено!");
                        return;
                    }
                    if (tbFirstName.Text.Replace(" ", "") == "")
                    {
                        MessageBox.Show("Поле имя должно быть заполнено!");
                        return;
                    }
                    if (tbUserName.Text.Replace(" ", "") == "")
                    {
                        MessageBox.Show("Поле логин должно быть заполнено!");
                        return;
                    }
                    Regex regex = new Regex("(?=.*[A-Z])"); // Регулярное выражение для проверки наличия 1 заглавного латинского символа
                    if (regex.IsMatch(pbPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Пароль должен содержать не менее 1 заглавного латинского символа", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Regex regex1 = new Regex("(?=.*[a-z].*[a-z].*[a-z])"); // Регулярное выражение для проверки наличия 3 строчных латинских символов
                    if (regex1.IsMatch(pbPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Пароль должен содержать не менее 3 строчных латинских символов", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Regex regex2 = new Regex("(?=.*[0-9].*[0-9])"); // Регулярное выражение для проверки наличия 2 цифр
                    if (regex2.IsMatch(pbPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Пароль должен содержать не менее 2 цифр", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Regex regexLength = new Regex(".{8,}"); // Регулярное выражение для проверки длины пароля
                    if (regex2.IsMatch(pbPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Общая длина пароля должна быть не менее 8 символов", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (cbPosition.Text == "")
                    {
                        MessageBox.Show("Поле должность должно быть выбрано из списка!");
                        return;
                    }
                    if (cbUserStates.Text == "")
                    {
                        MessageBox.Show("Поле состояние должно быть выбрано из списка!");
                        return;
                    }
                    if (GetProverkaLogin() == true)
                    {
                        MessageBox.Show("Пользователь с таким логином уже зарегистрирован!");
                        return;
                    }
                    if (IsCheckEmail(tbEmail.Text))
                    {
                        user = new Users();
                        user.LastName = tbLastName.Text;
                        user.FirstName = tbFirstName.Text;
                        if (tbMiddleName.Text == "")
                        {
                            user.MiddleName = null;
                        }
                        else
                        {
                            user.MiddleName = tbMiddleName.Text;
                        }
                        user.UserName = tbUserName.Text;
                        user.Password = pbPassword.Password;
                        user.RoleId = 2;
                        user.PositionId = cbPosition.SelectedIndex;
                        user.UserStateId = cbUserStates.SelectedIndex;
                        if (cbVisibility.IsChecked == false)
                        {
                            user.Visible = false;
                        }
                        else
                        {
                            user.Visible = true;
                        }
                        if (tbPhone.Text == "")
                        {
                            user.PhoneNumber = null;
                        }
                        else
                        {
                            user.PhoneNumber = tbPhone.Text;
                        }
                        user.Email = tbEmail.Text;
                        if (NewPath == null)
                        {
                            user.Photo = null;
                        }
                        else
                        {
                            user.Photo = NewPath.Substring(NewPath.LastIndexOf('\\')).Replace("\\", "");
                        }
                        if (tbScore.Text == "")
                        {
                            user.Score = 0;
                        }
                        else
                        {
                            user.Score = Convert.ToInt32(tbScore.Text);
                        }
                        if (tbLevel.Text == "")
                        {
                            user.Level = 1;
                        }
                        else
                        {
                            user.Level = Convert.ToInt32(tbLevel.Text);
                        }
                        if (tbXP.Text == "")
                        {
                            user.XP = 0;
                        }
                        else
                        {
                            user.XP = Convert.ToInt32(tbXP.Text);
                        }
                        DataBase.Base.Users.Add(user);
                        MessageBox.Show("Успешное добавление!");
                        DataBase.Base.SaveChanges();
                        Close();
                    }
                }
                else
                {
                    if (tbLastName.Text.Replace(" ", "") == "")
                    {
                        MessageBox.Show("Поле фамилия должно быть заполнено!");
                        return;
                    }
                    if (tbFirstName.Text.Replace(" ", "") == "")
                    {
                        MessageBox.Show("Поле имя должно быть заполнено!");
                        return;
                    }
                    if (tbUserName.Text.Replace(" ", "") == "")
                    {
                        MessageBox.Show("Поле логин должно быть заполнено!");
                        return;
                    }
                    Regex regex = new Regex("(?=.*[A-Z])"); // Регулярное выражение для проверки наличия 1 заглавного латинского символа
                    if (regex.IsMatch(pbPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Пароль должен содержать не менее 1 заглавного латинского символа", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Regex regex1 = new Regex("(?=.*[a-z].*[a-z].*[a-z])"); // Регулярное выражение для проверки наличия 3 строчных латинских символов
                    if (regex1.IsMatch(pbPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Пароль должен содержать не менее 3 строчных латинских символов", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Regex regex2 = new Regex("(?=.*[0-9].*[0-9])"); // Регулярное выражение для проверки наличия 2 цифр
                    if (regex2.IsMatch(pbPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Пароль должен содержать не менее 2 цифр", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Regex regexLength = new Regex(".{8,}"); // Регулярное выражение для проверки длины пароля
                    if (regex2.IsMatch(pbPassword.Password.ToString()) == false)
                    {
                        MessageBox.Show("Общая длина пароля должна быть не менее 8 символов", "Системное сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (cbPosition.Text == "")
                    {
                        MessageBox.Show("Поле роль должно быть выбрано из списка!");
                        return;
                    }
                    if (cbUserStates.Text == "")
                    {
                        MessageBox.Show("Поле роль должно быть выбрано из списка!");
                        return;
                    }
                    if (IsCheckEmail(tbEmail.Text))
                    {
                        user.LastName = tbLastName.Text;
                        user.FirstName = tbFirstName.Text;
                        if (tbMiddleName.Text == "")
                        {
                            user.MiddleName = null;
                        }
                        else
                        {
                            user.MiddleName = tbMiddleName.Text;
                        }
                        user.UserName = tbUserName.Text;
                        user.Password = pbPassword.Password;
                        user.PositionId = cbPosition.SelectedIndex + 1;
                        user.UserStateId = cbUserStates.SelectedIndex + 1;
                        if (cbVisibility.IsChecked == false)
                        {
                            user.Visible = false;
                        }
                        else
                        {
                            user.Visible = true;
                        }
                        if (tbPhone.Text == "")
                        {
                            user.PhoneNumber = null;
                        }
                        else
                        {
                            user.PhoneNumber = tbPhone.Text;
                        }
                        user.Email = tbEmail.Text;
                        if (NewPath != null)
                        {
                            user.Photo = NewPath.Substring(NewPath.LastIndexOf('\\')).Replace("\\", "");
                        }
                        if (tbScore.Text == "")
                        {
                            user.Score = Convert.ToInt32(tbScore.Text);
                        }
                        else
                        {
                            user.Score = Convert.ToInt32(tbScore.Text);
                        }
                        if (tbLevel.Text == "")
                        {
                            user.Level = Convert.ToInt32(tbLevel.Text);
                        }
                        else
                        {
                            user.Level = Convert.ToInt32(tbLevel.Text);
                        }
                        if (tbXP.Text == "")
                        {
                            user.XP = Convert.ToInt32(tbXP.Text);
                        }
                        else
                        {
                            user.XP = Convert.ToInt32(tbXP.Text);
                        }
                        DataBase.Base.SaveChanges();
                        MessageBox.Show("Успешное изменение!");
                        Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так", "Ошибка");
            }
        }
        /// <summary>
        /// Проверка на логин
        /// </summary>
        /// <returns></returns>
        bool GetProverkaLogin()
        {
            Users user = DataBase.Base.Users.FirstOrDefault(x => x.UserName == tbUserName.Text);
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void cbPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Positions> positions = DataBase.Base.Positions.ToList();
            if (cbPosition.SelectedIndex == (positions.Count + 1))
            {
                Window_AddPosition addPosition = new Window_AddPosition();
                addPosition.ShowDialog();

                List<Positions> position = DataBase.Base.Positions.ToList();
                cbPosition.Items.Remove("не выбрана должность");
                for (var i = 0; i < position.Count; i++)
                {
                    cbPosition.Items.Remove(position[i].Kind);
                }
                cbPosition.Items.Remove("Добавить должность");
                cbPosition.Items.Add("не выбрана должность");
                for (var i = 0; i < position.Count; i++)
                {
                    cbPosition.Items.Add(position[i].Kind);
                }
                cbPosition.Items.Add("Добавить должность");
                cbPosition.SelectedIndex = 0;
            }
        }

        private void tbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == "(") || (e.Text == ")") || (e.Text == "+") || (e.Text == "-")))
            {
                e.Handled = true;
            }
        }

        private void cbShowPassword_Click(object sender, RoutedEventArgs e)
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
