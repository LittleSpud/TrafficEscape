using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficEscape.GameLogic;

public class LaneManager
{
    public double[] LanePositions { get; } = new double[3];

    public void CalculateLanePositions(double width)
    {
        if (width <= 0)
            return;

        double laneWidth = width / 3;

        LanePositions[0] = laneWidth * 0.5;
        LanePositions[1] = laneWidth * 1.5;
        LanePositions[2] = laneWidth * 2.5;
    }
}
