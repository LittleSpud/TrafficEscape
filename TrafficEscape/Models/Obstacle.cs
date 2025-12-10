using System;

namespace TrafficEscapeGame.Models;

public class Obstacle
{
	public int Lane { get; set; }
	public Image Sprite { get; set; }
	public double Y { get; set; }
	public double Speed { get; set; }
}
