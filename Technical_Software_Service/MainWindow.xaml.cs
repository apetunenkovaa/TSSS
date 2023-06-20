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
using System.Windows.Threading;

namespace Technical_Software_Service
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int sec = 28800;
        private DispatcherTimer dispatcherTimer;
        public MainWindow()
        {
            InitializeComponent();
            DataBase.Base = new HelpdeskEntities();
            ClassFrame.MainF = Mframe;
            ClassFrame.MainF.Navigate(new Page_Authorization());
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += timerTick;
            dispatcherTimer.Start();
        }

        void timerTick(object sender, EventArgs e)
        {
            if (sec != 0)
            {
                sec--;
                int minutes = sec / 60;
                int newSec = sec - minutes * 60;
                int hour = minutes / 60;
                int newMinnutes = minutes - hour * 60;

                Timer.Text = $"До конца рабочего дня осталось:{hour}:{newMinnutes}:{newSec}";
            }
            else
            {
                dispatcherTimer.Stop();
                MessageBox.Show("Вы отработали 8 часов!", "Рабочий день окончен", MessageBoxButton.OK);
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int minutes = sec / 60;
            int newSec = sec - minutes * 60;
            int hour = minutes / 60;
            int newMinnutes = minutes - hour * 60;
            var res = MessageBox.Show($"Вы ещё не закончили свою работу на сегодня, осталось работать {hour}:{newMinnutes}:{newSec}, Вы уверены, что хотите выйти?", "Предупреждение", MessageBoxButton.YesNo);
            if(res == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                MessageBox.Show("Вы не отработали установленное количество времени", "Рабочий день окончен досрочно", MessageBoxButton.OK);
            }
        }
    }
}