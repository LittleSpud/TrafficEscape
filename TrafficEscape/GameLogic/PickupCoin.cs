using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficEscape.Models;

namespace TrafficEscape.GameLogic;

public class PickupCoin
{
    public Pickup Model { get; }

    public PickupCoin(Pickup pickup)
    {
        Model = pickup;
    }
}
