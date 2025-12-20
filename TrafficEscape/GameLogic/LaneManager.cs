using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficEscape.GameLogic;

public class LaneManager
{
    public double[] LanePositions { get; } = new double[3];
    public double TotalWidth { get; private set; }

    public void CalculateLanePositions(double roadWidth)
    {
        TotalWidth = roadWidth;
        double laneWidth = roadWidth / 3;

        LanePositions[0] = laneWidth * 0.95;
        LanePositions[1] = laneWidth * 1.5;
        LanePositions[2] = laneWidth * 2.1;
    }
    public double GetLaneX(int laneIndex)
    {
        double laneWidth = TotalWidth / 3;
        double centerX = (laneWidth * laneIndex) + (laneWidth / 2);

        return centerX - 40;
    }
}
