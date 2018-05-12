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
    public static class CinemaService
    {
       //get movies by room number
        public static List<CinemaModel> GetMoviesByRoom(int room)
        {
            List<CinemaModel> returnList = new List<CinemaModel>();
            for (int i=0; i< Data.Count; i++)
            {
                if (Data[i].Room == room)
                {
                    returnList.Add(Data[i]);
                }               
            }
            return returnList;
        }

        //get list of our data from CSV
        public static List<CinemaModel> Data { get; } = new List<CinemaModel>();

        //add new customer and ticket
        public static void AddEntry(CinemaModel entry){
            Data.Add(entry);
        }

        //delete ticket
        public static void DeleteEntry(CinemaModel entry){
            Data.Remove(entry);
        }
    }
}
