namespace TrafficEscape.Services;

public class DifficultyService
{
    public double SpeedMultiplier { get; private set; } = 0.7;
    public double PickupChance { get; private set; } = 0.10;

    public void IncreaseDifficulty()
    {
        SpeedMultiplier = Math.Min(SpeedMultiplier + 0.000005, 1.3);
        PickupChance = Math.Max(PickupChance - 0.005, 0.05);
    }
}

