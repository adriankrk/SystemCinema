using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Text.RegularExpressions;

namespace SystemCinema
{
    /// <summary>  
    ///  Important class read from CSV and save data to list
    /// </summary> 
    public class CinemaService
    {
        private static List<CinemaModel> data = new List<CinemaModel>();  //main list of our data

        /****   Delegate to convert from string  ******/
        delegate TypeOfTicket delegatTyp(string value);
        delegate Tuple<int, int> delagatSeat(string value);
    
        public static void ReadFromCSV(MainWindow Main_Window)
        {
            
            delegatTyp convert_type = (val) => { if (val == "sale") return TypeOfTicket.sale; else return TypeOfTicket.reservation; };
            delagatSeat convert_seat = (val) =>
            {
                switch (val.Length)
                {
                    case 6:
                        val = val.Substring(1, 3);
                        break;
                    case 7:
                        val = val.Substring(1, 4);
                        break;
                    case 8:
                        val = val.Substring(1, 5);
                        break;
                }; var newstr = val.Split('-');
                return new Tuple<int, int>(Convert.ToInt32(newstr[0]) - 1, Convert.ToInt32(newstr[1]) - 1);
            };


            try
            {
                using (var fs = File.OpenRead(@"D:\projects\DataInputGroupBxxegsrht.csv"))
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        /***** Check if data in CSV have correct length ****/
                        if (values.Length != 7)   
                        {
                            MessageBox.Show("Błąd długości danych - linia: " + values[1], "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                            continue;
                        }
                        /***********************************************/

                        /***** Variables to help check time, room number and seat have correct format or check if are dublets in our CSV****/                      
                        int room;
                        string pattern = @"(\(\d+\-\d+\))";
                        Match result = Regex.Match(values[6], pattern);
                        bool is_dublet = false;
                        /***********************************************/

                        /***********Check and add value from CSV file***************/
                        if ((values[2].Equals("sale") || values[2].Equals("reservation")) && int.TryParse(values[5], out room) && Int64.TryParse(values[0], out long time) && result.Success) //check if type of ticket, timestamp, room number, seat have correct format
                        {
                            /**********Check if are dublets in our CSV ********/
                            foreach(var item in data){
                                if (item.Name.Equals(values[3]) && item.Movie.Equals(values[4]) && item.Room.Equals(Convert.ToInt32(values[5])) && item.Seat.Equals(convert_seat(values[6])))
                                {
                                    MessageBox.Show("Błąd dublety - linia : " + values[1], "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                                    if (item.Timestamp > Convert.ToInt64(values[0]))       // if dublet was find check earlier timestamp
                                    {
                                        data.Remove(item);
                                        if (Convert.ToInt32(values[5]) >= 1 && Convert.ToInt32(values[5]) <= 4)       //check room number
                                        {
                                            CinemaModel oneclient = new CinemaModel(Convert.ToInt64(values[0]), convert_type(values[2]), values[3], values[4], Convert.ToInt32(values[5]), convert_seat(values[6]));
                                            data.Add(oneclient);
                                        }
                                        else                                       
                                            MessageBox.Show("Błąd - niepoprawny numer sali - linia: " + values[1], "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);                                       
                                    }
                                    is_dublet = true;                                    
                                    break;
                                }
                            }
                            /***********************************************/

                            if (!is_dublet)  // if dublet did not find we can add new item
                            {
                                if (Convert.ToInt32(values[5]) >= 1 && Convert.ToInt32(values[5]) <= 4)     //check room number
                                {
                                    CinemaModel oneclient = new CinemaModel(Convert.ToInt64(values[0]), convert_type(values[2]), values[3], values[4], Convert.ToInt32(values[5]), convert_seat(values[6]));
                                    data.Add(oneclient);
                                }
                                else                                
                                    MessageBox.Show("Błąd - niepoprawny numer sali - linia: " + values[1], "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);                                
                            }
                        }
                        else
                        {
                            MessageBox.Show("Błąd linia: " + values[1], "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                            continue;
                        }
                       /***********************************************/                            
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Brak pliku z danymi!!!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


       //get movies by room number
        public static List<CinemaModel> getMoviesByRoom(int room)
        {
            List<CinemaModel> returnList = new List<CinemaModel>();
            for (int i=0; i<data.Count; i++)
            {
                if (data[i].Room == room)
                {
                    returnList.Add(data[i]);
                }               
            }
            return returnList;
        }

        //get list of our data from CSV
        public List<CinemaModel> Data
        {
            get
            {
                return data;
            }
        }

        //add new customer and ticket
        public static void AddEntry(CinemaModel entry){
            data.Add(entry);
        }

        //delete ticket
        public static void deleteEntry(CinemaModel entry){
            data.Remove(entry);
        }
    }
}
