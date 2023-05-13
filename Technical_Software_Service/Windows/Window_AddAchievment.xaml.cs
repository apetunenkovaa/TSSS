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
        private Achievements achievement;
        bool flag;
        public string newFilePath = null; // Путь к картинке
        public Window_AddAchievment(Users user, Achievements achievement = null)
        {
            InitializeComponent();
            this.user = user;
            this.achievement = achievement;

            // Заполняем поля в окне редактирования текущими значениями достижения (если оно не null)
            if (achievement != null)
            {
                tbTitle.Text = achievement.Title;
                tbDescription.Text = achievement.Description;
                if (achievement.Image != null)
                {
                    string imagePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Image\\" + achievement.Image;
                    if (File.Exists(imagePath))
                    {
                        BitmapImage img = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                        imgAchievment.ImageSource = img;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbTitle.Text == "" || tbDescription.Text == "")
                {
                    MessageBox.Show("Обязательные поля не заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (achievement != null) // если редактирование существующего достижения
                    {
                        achievement.Title = tbTitle.Text;
                        achievement.Description = tbDescription.Text;
                        if (newFilePath != null)
                        {
                            achievement.Image = newFilePath.Substring(newFilePath.LastIndexOf('\\')).Replace("\\", "");
                        }
                        DataBase.Base.SaveChanges();
                        MessageBox.Show("Успешное изменение!");
                    }
                    else // если добавление нового достижения
                    {
                        Achievements newAchievement = new Achievements();
                        newAchievement.Title = tbTitle.Text;
                        if (tbDescription.Text == "")
                        {
                            newAchievement.Description = null;
                        }
                        else
                        {
                            newAchievement.Description = tbDescription.Text;
                        }
                        if (newFilePath == null)
                        {
                            newAchievement.Image = null;
                        }
                        else
                        {
                            newAchievement.Image = newFilePath.Substring(newFilePath.LastIndexOf('\\')).Replace("\\", "");
                        }
                        HelpdeskEntities.GetContext().Achievements.Add(newAchievement);
                        HelpdeskEntities.GetContext().SaveChanges();
                        MessageBox.Show("Успешное добавление!");
                        this.Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так..", "Системное сообшение");
            }
        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();
            string path = OFD.FileName;

            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Вы не выбрали фото");
                return;
            }


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
