namespace TrafficEscape.Services;

public class DifficultyService
{
    public double SpeedMultiplier { get; private set; } = 0.1;

    public void IncreaseDifficulty()
    {
        SpeedMultiplier += 0.5;
    }
}

