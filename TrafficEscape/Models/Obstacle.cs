using System;

namespace TrafficEscape.Models;

public class Obstacle
{
	public int Lane { get; set; }
    public required Image Sprite { get; set; }
	public double Y { get; set; }
	public double Speed { get; set; }
}
