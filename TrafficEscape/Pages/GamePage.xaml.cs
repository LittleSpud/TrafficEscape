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
        SizeChanged += OnPageSized;
    }
    private async void OnPageSized(object? sender, EventArgs e)
    {
        SizeChanged -= OnPageSized;
        await Task.Delay(50);

        laneManager.CalculateLanePositions(LaneGrid.Width);
        playerCar.CurrentLane = 1;
        PositionPlayerCar();

        gameLoop = new GameLoop(Dispatcher, UpdateGame);
        gameLoop.Start();
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

        PlayerCarView.TranslationX = 0;
        PlayerCarView.TranslationY = 0;
    }
    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        Loaded -= OnPageLoaded;
        await Task.Delay(50);

        if (LaneGrid == null || PlayerCarView == null)
            return;
        Console.WriteLine("GamePage Loaded");

        laneManager.CalculateLanePositions(Width);
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
    private async void MoveLeft()
    {
        if (isMoving || playerCar.CurrentLane <= 0) return;
       
        playerCar.CurrentLane--;
        PositionPlayerCar();
    }

    private async void MoveRight()
    {
        if (isMoving || playerCar.CurrentLane >= 2) return;
        playerCar.CurrentLane++;
        PositionPlayerCar();
    }

    private async Task AnimateCarToLane(int lane)
    {
        isMoving = true;

        double targetX =
            laneManager.LanePositions[lane] - (PlayerCarView.Width / 2);

        double currentX = PlayerCarView.X;

        double updatedX = targetX - currentX;

        await PlayerCarView.TranslateTo(
            updatedX - PlayerCarView.X,
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
