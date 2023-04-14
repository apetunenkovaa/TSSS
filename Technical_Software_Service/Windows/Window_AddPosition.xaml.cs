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
using System.Windows.Shapes;

namespace Technical_Software_Service
{
    /// <summary>
    /// Логика взаимодействия для Window_AddPosition.xaml
    /// </summary>
    public partial class Window_AddPosition : Window
    {
        public Window_AddPosition()
        {
            InitializeComponent();
        }

        private void btnCanel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tbPositions.Text == "")
            {
                MessageBox.Show("Обязательное поле не заполнено!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Positions positions = new Positions();
                positions.Kind = tbPositions.Text;
                DataBase.Base.Positions.Add(positions);
                DataBase.Base.SaveChanges();
                this.Close();
            }
        }
    }
}
