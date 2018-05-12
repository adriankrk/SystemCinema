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

namespace SystemCinema
{
    /// <summary>
    /// Interaction logic for Room2.xaml
    /// Class to select seat and sale ticket
    /// </summary>
    public partial class Room2 : Window, IRoom
    {
        private RoomMainForm main_form;                //variable with object of parrent window
        private bool availability;
        public List<Button> Buttons { get; }           //list with buttons, which are in grid

        public Room2(RoomMainForm main_form, bool availability)
        {
            InitializeComponent();
            Buttons = new List<Button>();
            this.availability = availability;
            this.main_form = main_form;
            FillButtonTable();
            RoomView.Fill_grid(main_form, this);   /*Fill grid of buttons (seats) with colours in dependency of its status********/
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
            main_form.Show();
        }

        /*******Create list with all buttons********/
        public void FillButtonTable()
        {
            for (int x = 0; x < 12; x++)
                for (int y = 0; y < 12; y++)
                    Buttons.Add(GetButton(x, y));
        }

        public Button GetButton(int x, int y)
        {
            string button_name = "button" + x.ToString() + "_" + y.ToString();
            var button = FindName(button_name);
            return (Button)button;
        }

        /******Get x and y of seat after button click and forward to form*****/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RoomView.ChooseSeat(sender, main_form, availability))
            {
                Close();
                main_form.Show();
            }
        }

        /****Display customer name and x,y of seat after mouse leave*****/
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            RoomView.DisplayTip(sender, main_form, availability);
        }

    }
}
