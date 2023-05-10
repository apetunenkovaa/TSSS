using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Technical_Software_Service
{
    /// <summary>
    /// Логика взаимодействия для Window_AddDailyTasks.xaml
    /// </summary>
    public partial class Window_AddDailyTasks : Window
    {
        Users user;
        DailyTasks daily;
        string newFilePath = null; // Путь к картинке
        public Window_AddDailyTasks(Users user)
        {
            InitializeComponent();
            this.user = user;

            List<TaskType> types = DataBase.Base.TaskType.ToList();
            foreach (var type in types)
            {
                cbTypeTask.Items.Add(type.TaskType1);
            }
            cbTypeTask.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbTitle.Text == "" || tbDescription.Text == "" || tbScore.Text == "" || tbXP.Text == "" || tbTotalCount.Text == "")
                {
                    MessageBox.Show("Обязательные поля не заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (cbTypeTask.SelectedItem == null) // Проверяем, было ли выбрано значение в ComboBox
                {
                    MessageBox.Show("Выберите тип задания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    daily = new DailyTasks();
                    daily.Title = tbTitle.Text;
                    if (tbDescription.Text == "")
                    {
                        daily.Description = null;
                    }
                    else
                    {
                        daily.Description = tbDescription.Text;
                    }
                    if (newFilePath == null)
                    {
                        daily.Image = null;
                    }
                    else
                    {
                        daily.Image = newFilePath.Substring(newFilePath.LastIndexOf('\\')).Replace("\\", "");
                    }
                    daily.Score = Convert.ToInt32(tbScore.Text);
                    daily.XP = Convert.ToInt32(tbXP.Text);
                    daily.TotalCount = Convert.ToInt32(tbTotalCount.Text);
                    daily.TaskTypeId = cbTypeTask.SelectedIndex + 1;
                    HelpdeskEntities.GetContext().DailyTasks.Add(daily);
                    HelpdeskEntities.GetContext().SaveChanges();
                    MessageBox.Show("Успешное добавление!");
                    this.Close();
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
