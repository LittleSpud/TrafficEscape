using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficEscape.Models;
public class EnemyCar
{
    public Obstacle Model { get; }

    public EnemyCar(Obstacle obstacle)
    {
        Model = obstacle;
    }

    public void Update(double speed)
    {
        Model.Y += speed;
    }
}
