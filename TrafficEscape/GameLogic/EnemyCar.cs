using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficEscape.Models;
using Microsoft.Maui.Controls;
namespace TrafficEscape.GameLogic;

public class EnemyCar
{
    public int Lane { get; set; }
    public double Y { get; set; }
    public double Speed { get; set; }
    public Image Sprite { get; set; } = null!;
}
