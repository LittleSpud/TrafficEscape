using System;

namespace TrafficEscape.Models;

public enum PickupType
{
    Coin, Fuel, Boost
}


public class Pickup
{
    public int Lane { get; set; }
    public Image Sprite { get; set; }
    public double Y { get; set; }
    public double Speed { get; set; }
    public PickupType Type { get; set; }
}
