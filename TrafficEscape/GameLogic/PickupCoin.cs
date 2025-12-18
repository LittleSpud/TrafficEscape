using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficEscape.Models;

namespace TrafficEscape.GameLogic;

public class PickupCoin
{
    public int Lane { get; set; }
    public double Y { get; set; }
    public double Speed { get; set; }
    public Image View { get; set; } = null!;

}
