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
    /// Interaction logic for Room1_Availability.xaml
    /// Class to select seat and sale ticket
    /// </summary>
    public partial class Room1_Availability : Window 
    {
        public Window Sala;                 //to close this window
        private List<Button> Buttons;       //list with buttons, which are in grid
        ToolTip t1 = new ToolTip();         //to display tip
        RoomMainForm MainFormSala1;         //variable with object of parrent window
        private bool availability;

        public Room1_Availability(bool availability)
        {
            InitializeComponent();
            Buttons = new List<Button>();
            FillButtonTable();
            this.availability = availability;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
            Sala.Show();
        }

        /*****Fill grid of buttons (seats) with colours in dependency of its status********/
        public void Fill_grid(RoomMainForm form)
        {
            this.MainFormSala1 = form;
            int[,] sala_pattern = form.getSalaForm.Room;
            for (int i = 0; i < sala_pattern.GetLength(0); i++)
            {
                for (int j = 0; j < sala_pattern.GetLength(1); j++)
                {
                    
                    if (sala_pattern[i, j] == 1)
                    {
                        string tmpButtonName = "button" + j.ToString() + "_" + i.ToString();
                        Button result = Buttons.Find(x => x.Name.ToString() == tmpButtonName);
                        if (result != null)
                        {
                            bool is_in_csv = false;
                            Tuple<int, int> t1 = new Tuple<int, int>(i, j);
                            foreach (var item in form.list_with_one_movie_only)
                            {
                                if (t1.Equals(item.Seat) && item.Type == TypeOfTicket.reservation)
                                {
                                    result.Background = Brushes.Yellow;
                                    is_in_csv = true;
                                }
                                if (t1.Equals(item.Seat) && item.Type == TypeOfTicket.sale)
                                {
                                    result.Background = Brushes.Red;
                                    is_in_csv = true;
                                }
                                
                            }
                            if (!is_in_csv || form.list_with_one_movie_only.Count == 0)
                            {
                                result.Background = Brushes.Green;
                            }
                        }
                            

                    }
                    else
                    {
                        string tmpButtonName = "button" + j.ToString() + "_" + i.ToString();
                        Button result = Buttons.Find(x => x.Name.ToString() == tmpButtonName);
                        if (result != null)
                            result.Background = Brushes.Gray;
                    }

                }
            }
        }
        /*******************************************/

        /*******Create list with all buttons********/
        private void FillButtonTable()
        {
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    Buttons.Add(getButton(x,y));
        }

        private Button getButton(int x, int y)
        {
            string button_name = "button" + x.ToString() + "_" + y.ToString();
            var button = this.FindName(button_name);
            return (Button) button;
        }

        /******Get x and y of seat after buuton click and forward to form*****/
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (availability)
            {
                if (b.Background == Brushes.Gray || b.Background == Brushes.Yellow || b.Background == Brushes.Red)
                {
                    MessageBox.Show("Nie można wybrać tego miejsca!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    if (b.Background == Brushes.Green)
                    {
                        string namebutton = b.Name;
                        namebutton = namebutton.Substring(6);
                        var newstr = namebutton.Split('_');
                        MainFormSala1.seat = new Tuple<int, int>(Convert.ToInt32(newstr[1]), Convert.ToInt32(newstr[0]));
                        MainFormSala1.SetLabel();
                        this.Close();
                        Sala.Show();
                    }
            }
            else
            {
                if (b.Background == Brushes.Gray || b.Background == Brushes.Green)
                {
                    MessageBox.Show("Nie można usunąć nieobsadzonych miejsc!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string namebutton = b.Name;
                    namebutton = namebutton.Substring(6);
                    var newstr = namebutton.Split('_');
                    Tuple<int, int> thiseat = new Tuple<int, int>(Convert.ToInt32(newstr[1]), Convert.ToInt32(newstr[0]));
                    var object_with_this_seat = MainFormSala1.list_with_one_movie_only.Find(x => x.Seat.Equals(thiseat));
                    MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć wpis o nazwie: " + object_with_this_seat.Name, "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        MainFormSala1.list_with_one_movie_only.Remove(object_with_this_seat);
                        MainFormSala1.sala_movies.Remove(object_with_this_seat);
                        CinemaService.deleteEntry(object_with_this_seat);
                        this.Close();
                        Sala.Show();
                    }
                }
            }
            
            
        }

        /****Display customer name and x,y of seat after mouse leave*****/
        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;   
            string namebutton = b.Name;
            namebutton = namebutton.Substring(6);
            var newstr = namebutton.Split('_');

            if (availability)
            {
                if (b.Background == Brushes.Gray)
                {
                    ToolTip tooltip = new ToolTip { Content = "Miejsce (" + (Convert.ToInt32(newstr[1]) + 1).ToString() + "," + (Convert.ToInt32(newstr[0]) + 1).ToString() + ") - Nie dostępne" };
                    b.ToolTip = tooltip;
                }

                if (b.Background == Brushes.Green)
                {
                    ToolTip tooltip = new ToolTip { Content = "Miejsce (" + (Convert.ToInt32(newstr[1]) + 1).ToString() + "," + (Convert.ToInt32(newstr[0]) + 1).ToString() + ") - Dostępne" };
                    b.ToolTip = tooltip;
                }

                if (b.Background == Brushes.Yellow)
                {
                    ToolTip tooltip = new ToolTip { Content = "Miejsce (" + (Convert.ToInt32(newstr[1]) + 1).ToString() + "," + (Convert.ToInt32(newstr[0]) + 1).ToString() + ") - Zarezerwowane" };
                    b.ToolTip = tooltip;
                }

                if (b.Background == Brushes.Red)
                {
                    ToolTip tooltip = new ToolTip { Content = "Miejsce (" + (Convert.ToInt32(newstr[1]) + 1).ToString() + "," + (Convert.ToInt32(newstr[0]) + 1).ToString() + ") - Sprzedane" };
                    b.ToolTip = tooltip;
                }
            }
            else
            {
                if (b.Background == Brushes.Gray)
                {
                    ToolTip tooltip = new ToolTip { Content = "Miejsce (" + (Convert.ToInt32(newstr[1]) + 1).ToString() + "," + (Convert.ToInt32(newstr[0]) + 1).ToString() + ") - Nie dostępne" };
                    b.ToolTip = tooltip;
                    //tooltip.IsOpen = true;
                }

                if (b.Background == Brushes.Green)
                {
                    ToolTip tooltip = new ToolTip { Content = "Miejsce (" + (Convert.ToInt32(newstr[1]) + 1).ToString() + "," + (Convert.ToInt32(newstr[0]) + 1).ToString() + ") - Dostępne" };
                    b.ToolTip = tooltip;
                    //tooltip.IsOpen = true;
                }

                if (b.Background == Brushes.Yellow)
                {
                    Tuple<int, int> thiseat = new Tuple<int, int>(Convert.ToInt32(newstr[1]), Convert.ToInt32(newstr[0]));
                    var object_with_this_seat = MainFormSala1.list_with_one_movie_only.Find(x => x.Seat.Equals(thiseat));
                    ToolTip tooltip = new ToolTip { Content = "Klient: " + object_with_this_seat.Name + "\nMiejsce (" + (Convert.ToInt32(newstr[1]) + 1).ToString() + "," + (Convert.ToInt32(newstr[0]) + 1).ToString() + ") - Zarezerwowane" };
                    b.ToolTip = tooltip;
                    //tooltip.IsOpen = true;
                }

                if (b.Background == Brushes.Red)
                {
                    Tuple<int, int> thiseat = new Tuple<int, int>(Convert.ToInt32(newstr[1]), Convert.ToInt32(newstr[0]));
                    var object_with_this_seat = MainFormSala1.list_with_one_movie_only.Find(x => x.Seat.Equals(thiseat));
                    ToolTip tooltip = new ToolTip { Content = "Klient: " + object_with_this_seat.Name + "\nMiejsce (" + (Convert.ToInt32(newstr[1]) + 1).ToString() + "," + (Convert.ToInt32(newstr[0]) + 1).ToString() + ") - Sprzedane" };
                    b.ToolTip = tooltip;
                    //tooltip.IsOpen = true;
                }
            }
            
            
        }

       

       

    }
}
