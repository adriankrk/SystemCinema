using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SystemCinema
{
    public interface IRoom
    {
        List<Button> Buttons { get; }           //list with buttons, which are in grid
        void FillButtonTable();
        Button GetButton(int x, int y);
    }
}
