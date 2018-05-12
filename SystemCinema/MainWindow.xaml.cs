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

namespace SystemCinema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// The main class for startup Window of our program
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Sala1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RoomMainForm room = new RoomMainForm(1, this);
            OpenWindow(room);
        }

        private void Sala2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RoomMainForm room = new RoomMainForm(2, this);
            OpenWindow(room);
        }

        private void Sala3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RoomMainForm room = new RoomMainForm(3, this);
            OpenWindow(room);
        }

        private void Sala4_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RoomMainForm room = new RoomMainForm(4, this);
            OpenWindow(room);
        }

        //we open next window with parameter of room number
        private void OpenWindow(RoomMainForm room){
            room.Show();
            Hide();
        }
    }
}
