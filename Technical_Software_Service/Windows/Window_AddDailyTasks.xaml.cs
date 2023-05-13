using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
        public string newFilePath = null; // Путь к картинке
        public Window_AddDailyTasks(Users user, DailyTasks dailyTasks = null)
        {
            InitializeComponent();
            this.user = user;
            this.daily = dailyTasks;

            List<TaskType> types = DataBase.Base.TaskType.ToList();
            foreach (var type in types)
            {
                cbTypeTask.Items.Add(type.TaskType1);
            }
            cbTypeTask.SelectedIndex = 0;

            // Заполняем поля в окне редактирования текущими значениями достижения (если оно не null)
            if (dailyTasks != null)
            {
                tbTitle.Text = dailyTasks.Title;
                tbDescription.Text = dailyTasks.Description;
                tbXP.Text = Convert.ToString(dailyTasks.XP);
                tbScore.Text = Convert.ToString(dailyTasks.Score);
               tbTotalCount.Text = Convert.ToString(dailyTasks.TotalCount);

                if (dailyTasks.Image != null)
                {
                    string imagePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Image\\" + dailyTasks.Image;
                    if (File.Exists(imagePath))
                    {
                        BitmapImage img = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                        imgDailyTasks.ImageSource = img;
                    }
                }
            }
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
                    if (daily != null) // если редактирование существующего достижения
                    {
                        daily.Title = tbTitle.Text;
                        daily.Description = tbDescription.Text;
                        daily.XP = Convert.ToInt32(tbXP.Text);
                        daily.Score = Convert.ToInt32(tbScore.Text);
                        daily.TotalCount = Convert.ToInt32(tbTotalCount.Text);
                        string taskTypeName = (string)cbTypeTask.SelectedItem;
                        TaskType taskType = DataBase.Base.TaskType.FirstOrDefault(tt => tt.TaskType1 == taskTypeName);
                        daily.TaskTypeId = taskType.Id;

                        if (newFilePath != null)
                        {
                            daily.Image = newFilePath.Substring(newFilePath.LastIndexOf('\\')).Replace("\\", "");
                        }
                        DataBase.Base.SaveChanges();
                        MessageBox.Show("Успешное изменение!");
                    }
                    else // если добавление нового достижения
                    {
                        DailyTasks newDailyTasks = new DailyTasks();
                        newDailyTasks.Title = tbTitle.Text;
                        newDailyTasks.Score = Convert.ToInt32(tbScore.Text);
                        newDailyTasks.XP = Convert.ToInt32(tbXP.Text);
                        newDailyTasks.TotalCount = Convert.ToInt32(tbTotalCount.Text);
                        newDailyTasks.TaskTypeId = cbTypeTask.SelectedIndex + 1;

                        if (tbDescription.Text == "")
                        {
                            newDailyTasks.Description = null;
                        }
                        else
                        {
                            newDailyTasks.Description = tbDescription.Text;
                        }
                        if (newFilePath == null)
                        {
                            newDailyTasks.Image = null;
                        }
                        else
                        {
                            newDailyTasks.Image = newFilePath.Substring(newFilePath.LastIndexOf('\\')).Replace("\\", "");
                        }
                        HelpdeskEntities.GetContext().DailyTasks.Add(newDailyTasks);
                        HelpdeskEntities.GetContext().SaveChanges();
                        MessageBox.Show("Успешное добавление!");
                        this.Close();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                var errorMsg = "Ошибка при обновлении записей: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMsg += "\n\n" + ex.InnerException.Message;
                }
                MessageBox.Show(errorMsg, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                imgDailyTasks.ImageSource = img;
            }
        }
    }
}
