using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficEscape.Models;

    public class DifficultyManager
    {
        public double DifficultyLevel { get; private set; } = 1.0;
        public double ObstacleSpeed => Math.Max(600 - (DifficultyLevel * 50), 150);
        public double PickupChance => Math.Min(DifficultyLevel * 0.05, 0.30);

        public void IncreaseDifficulty()
        {
            DifficultyLevel += 0.05;
        }
    }

