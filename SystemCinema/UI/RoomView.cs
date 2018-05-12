using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SystemCinema
{
    static class RoomView
    {
        public static void Fill_grid(RoomMainForm main_form, IRoom room)
        {
            int[,] sala_pattern = main_form.GetRoomForm.Room;
            for (int i = 0; i < sala_pattern.GetLength(0); i++)
            {
                for (int j = 0; j < sala_pattern.GetLength(1); j++)
                {
                    if (sala_pattern[i, j] == 1)
                    {
                        string tmpButtonName = "button" + j.ToString() + "_" + i.ToString();
                        Button result = room.Buttons.Find(x => x.Name.ToString() == tmpButtonName);
                        if (result != null)
                        {
                            bool isFree = true;
                            Tuple<int, int> t1 = new Tuple<int, int>(i, j);
                            foreach (var item in main_form.ListWithOneMovieOnly)
                            {
                                if (t1.Equals(item.Seat) && item.Type == TicketType.reservation)
                                {
                                    result.Background = Brushes.Yellow;
                                    isFree = false;
                                }
                                if (t1.Equals(item.Seat) && item.Type == TicketType.sale)
                                {
                                    result.Background = Brushes.Red;
                                    isFree = false;
                                }

                            }
                            if ( isFree || main_form.ListWithOneMovieOnly.Count == 0)
                            {
                                result.Background = Brushes.Green;
                            }
                        }


                    }
                    else
                    {
                        string tmpButtonName = "button" + j.ToString() + "_" + i.ToString();
                        Button result = room.Buttons.Find(x => x.Name.ToString() == tmpButtonName);
                        if (result != null)
                            result.Background = Brushes.Gray;
                    }

                }
            }
        }

        public static bool ChooseSeat(object sender, RoomMainForm main_form, bool availability)
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
                        main_form.Seat = new Tuple<int, int>(Convert.ToInt32(newstr[1]), Convert.ToInt32(newstr[0]));
                        main_form.SetSeatLabel();
                        return true;                      
                    }

                return false;
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
                    var object_with_this_seat = main_form.ListWithOneMovieOnly.Find(x => x.Seat.Equals(thiseat));
                    MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć wpis o nazwie: " + object_with_this_seat.Name, "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        main_form.ListWithOneMovieOnly.Remove(object_with_this_seat);
                        main_form.RoomMovies.Remove(object_with_this_seat);
                        CinemaService.DeleteEntry(object_with_this_seat);
                        return true;
                    }
                }
                return false;
            }
        }

        public static void DisplayTip(object sender, RoomMainForm main_form, bool availability)
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
                }

                if (b.Background == Brushes.Green)
                {
                    ToolTip tooltip = new ToolTip { Content = "Miejsce (" + (Convert.ToInt32(newstr[1]) + 1).ToString() + "," + (Convert.ToInt32(newstr[0]) + 1).ToString() + ") - Dostępne" };
                    b.ToolTip = tooltip;
                }

                if (b.Background == Brushes.Yellow)
                {
                    Tuple<int, int> thiseat = new Tuple<int, int>(Convert.ToInt32(newstr[1]), Convert.ToInt32(newstr[0]));
                    var object_with_this_seat = main_form.ListWithOneMovieOnly.Find(x => x.Seat.Equals(thiseat));
                    ToolTip tooltip = new ToolTip { Content = "Klient: " + object_with_this_seat.Name + "\nMiejsce (" + (Convert.ToInt32(newstr[1]) + 1).ToString() + "," + (Convert.ToInt32(newstr[0]) + 1).ToString() + ") - Zarezerwowane" };
                    b.ToolTip = tooltip;
                }

                if (b.Background == Brushes.Red)
                {
                    Tuple<int, int> thiseat = new Tuple<int, int>(Convert.ToInt32(newstr[1]), Convert.ToInt32(newstr[0]));
                    var object_with_this_seat = main_form.ListWithOneMovieOnly.Find(x => x.Seat.Equals(thiseat));
                    ToolTip tooltip = new ToolTip { Content = "Klient: " + object_with_this_seat.Name + "\nMiejsce (" + (Convert.ToInt32(newstr[1]) + 1).ToString() + "," + (Convert.ToInt32(newstr[0]) + 1).ToString() + ") - Sprzedane" };
                    b.ToolTip = tooltip;
                }
            }
        }
    }
}
