using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemCinema
{
    /// <summary>  
    ///  Class to create room and fill 1 (is seat) or 0 (no seat)
    /// </summary> 
    public class RoomPattern
    {
        public RoomPattern(int dimension, int out_x, int out_y)   // out_ means that seats are not available
        {
            Room = new int[dimension, dimension];
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (i < out_y && j < out_x)
                    {
                        Room[i, j] = 0;
                    }
                    else
                    {
                        Room[i, j] = 1;
                    }
                }
            }
        }

        //get pattern of room
        public int[,] Room { get; }
    }
}
