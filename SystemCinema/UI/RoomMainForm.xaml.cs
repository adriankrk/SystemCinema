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
using System.IO;

namespace SystemCinema
{
    /// <summary>
    /// Interaction logic for Sala1.xaml
    /// Class to manage customers and open new window with rooms availibity
    /// </summary>
    public partial class RoomMainForm : Window
    {
        private Window mainWindow;
        private string movie;                                         //one selected movie from ListBox of all movies
        private readonly int roomNumber;

        public List<CinemaModel> RoomMovies { get; private set; }     //list of movies and seats, which are only in selected room
        public List<CinemaModel> ListWithOneMovieOnly { get; }        //list of seat per one movie
        public Tuple<int, int> Seat { get; set; }
        public RoomPattern GetRoomForm { get; private set; }

        public RoomMainForm(int roomNumber, MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.roomNumber = roomNumber;
            InitRoom(roomNumber);
            PrintMovies();
            SetMainLabel(roomNumber);
            ListWithOneMovieOnly = new List<CinemaModel>();          
        }

        /********Get movies from selected room and print to ListBox***********/
        private void PrintMovies()   
        {
            ListBoxMovies.Items.Clear();
            RoomMovies = CinemaService.GetMoviesByRoom(roomNumber);  
            foreach (var it in RoomMovies)
            {
                bool is_in_list = false;
                foreach (var it1 in ListBoxMovies.Items)
                {
                    if (it1.ToString() == it.Movie)
                    {
                        is_in_list = true;
                        break;
                    }
                }
                if (!is_in_list)
                    ListBoxMovies.Items.Add(it.Movie);
            }
        }
        /********************************************************************/

        /************Initialize room with exlude chairs in dependency of room number**************/
        private void InitRoom(int numberOfroom){          
            switch (numberOfroom)
            {
                case 1:                 
                    GetRoomForm = new RoomPattern(10, 5, 5);     // (dimension of room, lack of seat to x, lack of seat to y)
                    break;                   
                case 2:
                    GetRoomForm = new RoomPattern(12, 4, 6);
                    break;
                case 3:
                    GetRoomForm = new RoomPattern(16, 9, 5);
                    break;
                case 4:
                    GetRoomForm = new RoomPattern(20, 6, 8);
                    break;
            }
            
        }
        /******************************************************************************/       

        /*************Function for add new movie Button********************/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Movie.Text.Equals(""))
                MessageBox.Show("Nie podano nazwy filmu!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                ListBoxMovies.Items.Add(Movie.Text);
                Movie.Clear();
            }
        }
        /*******************************************************/

        /******* Read movie from ListBox and select movies******/
        private void Read_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxMovies.SelectedIndex == -1)
                MessageBox.Show("Nie zaznaczono żadnego filmu!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                ListWithOneMovieOnly.Clear();
                movie = ListBoxMovies.SelectedItem.ToString();
                movie_read.Content = movie;
                foreach (var it in RoomMovies)
                {
                    if (movie == it.Movie)
                    {
                        ListWithOneMovieOnly.Add(it);    //filter seats for selected movie
                    }
                }
            }
            
        }
        /**************************************************/

        /******Button to select seat for new customer****/
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (movie_read.Content.Equals(""))
            {
                MessageBox.Show("Nie wczytano żadnego filmu!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.OpenNewWindow(roomNumber, true);                //1-add new client window, 0 -delete client window
            }
            
        }
        /***********************************************/

        /******Button to add a new client and sale a ticket****/
        private void AddButton_Click_2(object sender, RoutedEventArgs e)
        {
            if (movie_read.Content.Equals("") || typeCombo.Text.ToString().Equals("") || NameTextBox.Text.ToString().Equals("") || seatLabel.Content.Equals(""))
            {
                MessageBox.Show("Wszytkie pola muszą być uzupełnione!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TicketType convert_type(string val)
                {
                    switch (val)
                    {
                        case "sprzedaż":
                            return TicketType.sale;
                        case "rezerwacja":
                            return TicketType.reservation;
                        default:
                            return TicketType.reservation;

                    }
                }

                long epochTicks = new DateTime(1970, 1, 1).Ticks;
                long unixTime = ((DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond);
                string type = typeCombo.Text.ToString();

                /****New entry to our list of ticket*********/
                CinemaModel entry = new CinemaModel(unixTime, convert_type(type), NameTextBox.Text.ToString(), movie_read.Content.ToString(), roomNumber, Seat);
                CinemaService.AddEntry(entry);
              
                RoomMovies.Add(entry);
                ListWithOneMovieOnly.Add(entry);

                /********Clear labels*****/
                typeCombo.Text = "";
                NameTextBox.Clear();
                seatLabel.Content = "";
            }
        }
        /*********************************************************/

        /******Button to open window to select seat and ticket, which will be delete****/
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (movie_read.Content.Equals(""))
            {
                MessageBox.Show("Nie wczytano żadnego filmu!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                OpenNewWindow(roomNumber, false);  //1-add new client, 0 -delete client
            }
        }
        /***********************************************/
        
        /********Open new window with seats availibity************/
        private void OpenNewWindow(int room_number, bool delOradd)
        {
                switch (room_number)                        //open window in dependency of room number
                {
                    case 1:
                        {
                            Room1 room = new Room1(this, delOradd);
                            room.Show();
                            Hide();
                            break;
                        }
                    case 2:
                        {
                            Room2 room = new Room2(this, delOradd);
                            room.Show();
                            Hide();
                            break;
                        }
                    case 3:
                        {
                            Room3 room = new Room3(this, delOradd);
                            room.Show();
                            Hide();
                            break;
                        }
                    case 4:
                        {
                            Room4 room = new Room4(this, delOradd);
                            room.Show();
                            Hide();
                            break;
                        }
                }         
   
        }
        /**************************************************************/ 
        
        /***************Generate raport for movies*********************/
        private void Raport_Button_Click(object sender, RoutedEventArgs e)
        {
            //tmp list beceuse we will check, increment licznik variable and delete entry
            List<CinemaModel> filmy_raport = new List<CinemaModel>();
            foreach (var item in RoomMovies)
            {
                filmy_raport.Add(item);
            }

            List<Tuple<string, int>> count = new List<Tuple<string, int>>();  //tuple (movie name, count of this movie repetition)
            
            while(filmy_raport.Any())
            {
                //get the first movie in our list
                string x = filmy_raport[0].Movie;
                int licznik = 0;

                //check and delete if seat is not in our room
                if (!CheckIfSeatIsInRoom(roomNumber, filmy_raport, 0))
                {
                    filmy_raport.RemoveAt(0);
                    continue;
                }
                else
                {
                    filmy_raport.RemoveAt(0);
                    licznik = 1;
                }
                
                //check other seats if is equal to first in list
                for (int i = 0; i < filmy_raport.Count; i++)
                {
                    if (!CheckIfSeatIsInRoom(roomNumber, filmy_raport, i))
                    {
                        filmy_raport.RemoveAt(i);
                        i--;
                        continue;
                    }

                    if (filmy_raport[i].Movie.Equals(x))
                    {
                        licznik++;
                        filmy_raport.RemoveAt(i);
                        i--;
                    }
                    else
                        continue;
                }

                //add to new list movie with count of repetition
                Tuple<string, int> tmp = new Tuple<string, int>(x, licznik);
                count.Add(tmp);
            }

            var result = count.OrderByDescending(x => x.Item2).ToList();     //sort our list

            /*******Generate raport******/
            string filename = "seanse_sala_" + roomNumber.ToString() + ".txt";
            StreamWriter file = new System.IO.StreamWriter(filename);
            foreach (var line in result)
                file.WriteLine(line);
            file.Close();
            MessageBox.Show("Raport wygenerowany", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }

        /*******************************************/

        /***************Generate raport for seats*********************/
        private void Raport_Button_Copy_Click(object sender, RoutedEventArgs e)
        {
            //tmp list beceuse we will check, increment licznik variable and delete entry
            List<CinemaModel> miejsca_raport = new List<CinemaModel>();
            foreach(var item in RoomMovies){
                miejsca_raport.Add(item);
            }

            List<Tuple<Tuple<int, int>, int>> count = new List<Tuple<Tuple<int, int>, int>>();   //tuple (seat x-y, count of this seat repetition)
            
            while (miejsca_raport.Any())
            {
                //get the first seat in our list
                var x = miejsca_raport[0].Seat;
                miejsca_raport.RemoveAt(0);
                int licznik = 0;

                //check and delete if seat is not in our room
                if (!CheckIfSeatIsInRoom(roomNumber, miejsca_raport, 0))
                {
                    miejsca_raport.RemoveAt(0);
                    continue;
                }
                else
                {
                    miejsca_raport.RemoveAt(0);
                    licznik = 1;
                }

                //check other seats if is equal to first in list
                for (int i = 0; i < miejsca_raport.Count; i++)
                {
                    if (!CheckIfSeatIsInRoom(roomNumber, miejsca_raport, i))
                    {
                        miejsca_raport.RemoveAt(i);
                        i--;
                        continue;
                    }

                    if (miejsca_raport[i].Seat.Equals(x))
                    {
                        licznik++;
                        miejsca_raport.RemoveAt(i);
                        i--;
                    }
                    else
                        continue;
                }

                //add to new list of seats with count of repetition
                Tuple<Tuple<int, int>, int> tmp = new Tuple<Tuple<int, int>, int>(x, licznik);
                count.Add(tmp);
            }

            var result = count.OrderByDescending(x => x.Item2).ToList();          //sort our list

            /*******Generate raport******/
            string filename = "miejsca_sala_" + roomNumber.ToString() + ".txt";
            StreamWriter file = new System.IO.StreamWriter(filename);
            foreach (var line in result)
                file.WriteLine("(" + (line.Item1.Item1 + 1).ToString() + "," + (line.Item1.Item2 + 1).ToString() + "): " + line.Item2);
            file.Close();
            MessageBox.Show("Raport wygenerowany", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /*****************Check if x and y are less than dimension of room and if seat is 1 or 0***********/
        private bool CheckIfSeatIsInRoom(int room_number, List<CinemaModel> list, int i)
        {
             switch (roomNumber)
             {
                 case 1:
                     if (list[i].Seat.Item1 > 9 || list[i].Seat.Item2 > 9 || GetRoomForm.Room[list[i].Seat.Item1, list[i].Seat.Item2] == 0)
                         return false;
                     else
                         return true;
                 case 2:
                     if (list[i].Seat.Item1 > 11 || list[i].Seat.Item2 > 11 || GetRoomForm.Room[list[i].Seat.Item1, list[i].Seat.Item2] == 0)
                        return false;
                     else
                         return true;
                 case 3:
                     if (list[i].Seat.Item1 > 11 || list[i].Seat.Item2 > 15 || GetRoomForm.Room[list[i].Seat.Item1, list[i].Seat.Item2] == 0)
                        return false;
                     else
                         return true;
                 case 4:
                     if (list[i].Seat.Item1 > 11 || list[i].Seat.Item2 > 19 || GetRoomForm.Room[list[i].Seat.Item1, list[i].Seat.Item2] == 0)
                        return false;
                     else
                         return true;
                 default:
                     return true;
             }                    
        }
        /***************************************************/

        public void SetSeatLabel()
        {
            if (Seat != null)
                seatLabel.Content = (Seat.Item1 + 1).ToString() + "," + (Seat.Item2 + 1).ToString();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
            mainWindow.Show();
        }

        private void SetMainLabel(int room_number)
        {
            MainLabel.Content += room_number.ToString();    //write room number to main label (title)
        }
    }
}
