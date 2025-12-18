using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficEscape.Services;

namespace TrafficEscape.GameLogic;
  
public class Spawner
{
    private readonly Random rng = new();
    private readonly DifficultyService difficulty;

    public Spawner (DifficultyService difficulty)
    {
        this.difficulty = difficulty;
    }

    public EnemyCar CreateEnemy(int lane)
    {
        return new EnemyCar
        {
            Lane = lane,
            Y = -200,
            Speed = 250 * difficulty.SpeedMultiplier
        };
    }
    public PickupCoin CreatePickup(int lane)
    {
        return new PickupCoin
        {
            Lane = lane,
            Y = -100,
            Speed = 200
        };
    }
}
