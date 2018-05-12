using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemCinema
{
    public enum TicketType {sale, reservation};
    
    /// <summary>  
    ///  Main class of our program for data from CSV 
    /// </summary>  
    public class CinemaModel
    {
        private int room;

        public CinemaModel(long timestamp, TicketType type, string name, string movie, int room, Tuple<int, int> seat)
        {
            Timestamp = timestamp;
            Type = type;
            Name = name;
            Movie = movie;
            this.room = room;
            Seat = seat;
        }

        public int Room
        {
            set
            {
                if (value >=1 && value <= 4)
                    room = value;
                else
                    MessageBox.Show("Niepoprawny numer sali!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }           
            get
            {
                return room;
            }
        }
        public string Movie { set; get; }

        public Tuple<int, int> Seat { set; get; }

        public TicketType Type { set; get; }

        public string Name { set; get; }

        public long Timestamp { set; get; }
    }
}
