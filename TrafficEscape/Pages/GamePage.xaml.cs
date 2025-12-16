using TrafficEscape.GameLogic;

namespace TrafficEscape.Pages;

public partial class GamePage : ContentPage
{
    private readonly PlayerCar playerCar = new();
    private readonly LaneManager laneManager = new();
    private bool isMoving;
    public GamePage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        base.OnAppearing();

        Loaded += OnPageLoaded;
    }
    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        Loaded -= OnPageLoaded;
        await Task.Delay(50);

        if (LaneGrid == null)
            return;
        Console.WriteLine("GamePage Loaded");

        laneManager.CalculateLanePositions(LaneGrid.Width);
        PositionPlayerCar();
    }

    private void InitializeLanes()
    {
        laneManager.CalculateLanePositions(LaneGrid.Width);
        PositionPlayerCar();
    }

    private void PositionPlayerCar()
    {
        double x = laneManager.LanePositions[playerCar.CurrentLane];

        AbsoluteLayout.SetLayoutBounds(
            PlayerCar,
            new Rect(
                x - PlayerCar.Width / 2,
                Height * 0.75,
                PlayerCar.Width,
                PlayerCar.Height
            )
        );
    }

    private async void MoveLeft()
    {
        if (isMoving || playerCar.CurrentLane <= 0) return;
        playerCar.CurrentLane--;
        await AnimateCarToLane(playerCar.CurrentLane);
    }

    private async void MoveRight()
    {
        if (isMoving || playerCar.CurrentLane >= 2) return;
        playerCar.CurrentLane++;
        await AnimateCarToLane(playerCar.CurrentLane);
    }

    private async Task AnimateCarToLane(int lane)
    {
        isMoving = true;

        double targetX =
            laneManager.LanePositions[lane] - (PlayerCar.Width / 2);

        await PlayerCar.TranslateTo(
            targetX - PlayerCar.X,
            0,
            150,
            Easing.CubicOut
        );

        PositionPlayerCar();
        isMoving = false;
    }

    private async void OnPause(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PausePage));
    }

    private void OnLeftTapped(object sender, TappedEventArgs e) => MoveLeft();
    private void OnRightTapped(object sender, TappedEventArgs e) => MoveRight();

    private void OnLeftSwipe(object sender, SwipedEventArgs e)
    {
        MoveLeft();
    }

    private void OnRightSwipe(object sender, SwipedEventArgs e)
    {
        MoveRight();
    }
}
