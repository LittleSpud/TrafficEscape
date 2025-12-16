using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using TrafficEscape.GameLogic;
using TrafficEscape.Services;
namespace TrafficEscape.Pages;

public partial class GamePage : ContentPage
{
    private readonly PlayerCar playerCar = new();
    private readonly LaneManager laneManager = new();

    private GameLoop? gameLoop;

    private bool isMoving;
    private double score;
    public GamePage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Loaded += OnPageLoaded;
    }
    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        Loaded -= OnPageLoaded;
        await Task.Delay(50);
        PlayerCarView.BackgroundColor = Colors.Red;

        if (LaneGrid == null || PlayerCarView == null)
            return;
        Console.WriteLine("GamePage Loaded");

        laneManager.CalculateLanePositions(LaneGrid.Width);
        playerCar.CurrentLane = 1;
        PositionPlayerCar();

        gameLoop = new GameLoop(Dispatcher, UpdateGame);
        gameLoop.Start();
    }
    private void UpdateGame(double update)
    {
        score += update * 10;
        ScoreLabel.Text = ((int)score).ToString();
    }
    private void PositionPlayerCar()
    {
        double x = laneManager.LanePositions[playerCar.CurrentLane];

        AbsoluteLayout.SetLayoutBounds(
            PlayerCarView,
            new Rect(
                x - PlayerCarView.Width / 2,
                Height * 0.75,
                PlayerCarView.Width,
                PlayerCarView.Height
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
            laneManager.LanePositions[lane] - (PlayerCarView.Width / 2);

        await PlayerCarView.TranslateTo(
            targetX - PlayerCarView.X,
            0,
            150,
            Easing.CubicOut
        );

        PositionPlayerCar();
        isMoving = false;
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
    private async void OnPause(object sender, EventArgs e)
    {
        gameLoop?.Stop();
        await Shell.Current.GoToAsync(nameof(PausePage));
    }
    private async void GameOver()
    {
        gameLoop?.Stop();

        SaveService.HighScore =
            Math.Max(SaveService.HighScore, (int)score);

        await Shell.Current.GoToAsync("//MainMenuPage");
    }
}
