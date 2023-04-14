using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для Window_AddAchievment.xaml
    /// </summary>
    public partial class Window_AddAchievment : Window
    {
        Users user;
        Achievements achievm;
        bool flag;
        string newFilePath = null; // Путь к картинке
        public Window_AddAchievment(Users user)
        {
            InitializeComponent();
            this.user = user;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            if (tbTitle.Text == "" || tbDescription.Text == "")
            {
                MessageBox.Show("Обязательные поля не заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {

                achievm = new Achievements();
                achievm.Title = tbTitle.Text;
                if (tbDescription.Text == "")
                {
                    achievm.Description = null;
                }
                else
                {
                    achievm.Description = tbDescription.Text;
                }
                if (newFilePath == null)
                {
                    achievm.Image = null;
                }
                else
                {
                    achievm.Image = newFilePath;
                }
                HelpdeskEntities.GetContext().Achievements.Add(achievm);
                HelpdeskEntities.GetContext().SaveChanges();
                //UserAchievements userAchievements = new UserAchievements();
                //userAchievements.AchievementID = achievm.Id;
                //userAchievements.UserId = user.Id;
                //DataBase.Base.UserAchievements.Add(userAchievements);
                MessageBox.Show("Успешное добавление!");
                this.Close();
            }
            //}
            //catch
            //{
            //    MessageBox.Show("Что-то пошло не так..", "Системное сообшение");
            //}         
        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();
            string path = OFD.FileName;
            if (path != null)
            {
                newFilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Image\\" + System.IO.Path.GetFileName(path); // Путь куда копировать файл
                if (!File.Exists(newFilePath)) // Проверка наличия картинки в папке
                {
                    File.Copy(path, newFilePath);
                }
                else
                {
                    MessageBox.Show("Такая картинка уже есть! Добавлено старое фото");
                }
                BitmapImage img = new BitmapImage(new Uri(newFilePath, UriKind.RelativeOrAbsolute));
                imgAchievment.ImageSource = img;
            }
        }
    }
}
