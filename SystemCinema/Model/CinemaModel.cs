using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystemCinema
{
    public enum TypeOfTicket {sale, reservation};
    
    /// <summary>  
    ///  Main class of our program for data from CSV 
    /// </summary>  
    public class CinemaModel
    {
        private long timestamp;
        private TypeOfTicket type;
        private string name;
        private string movie;
        private int room;
        private Tuple<int, int> seat;


        public CinemaModel(long timestamp, TypeOfTicket type, string name, string movie, int room, Tuple<int, int> seat)
        {
            this.timestamp = timestamp;
            this.type = type;
            this.name = name;
            this.movie = movie;
            this.room = room;
            this.seat = seat;
        }

        public int Room
        {
            set
            {
                if (value >=1 && value <= 4)
                    this.room = value;
                else
                    MessageBox.Show("Niepoprawny numer sali!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }           
            get
            {
                return this.room;
            }
        }
        public string Movie
        {
            set
            {
                this.movie = value;
            }
            get
            {
                return this.movie;
            }
        }

        public Tuple<int, int> Seat
        {
            set
            {
                this.seat = value;
            }
            get
            {
                return this.seat;
            }
        }

        public TypeOfTicket Type
        {
            set
            {
                this.type = value;
            }
            get
            {
                return this.type;
            }
        }

        public string Name
        {
            set
            {
                this.name = value;
            }
            get
            {
                return this.name;
            }
        }

        public long Timestamp
        {
            set
            {
                this.timestamp = value;
            }
            get
            {
                return this.timestamp;
            }
        }
    }
}
