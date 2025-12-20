using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficEscape.GameLogic;
    public class PlayerCar
    {
        public int CurrentLane { get; set; } = 1;
        public View? View { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

    public PlayerCar() { }
    public PlayerCar(View view)
    {
            View = view;
    }
}

