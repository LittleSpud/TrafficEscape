using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using TrafficEscape.GameLogic;
using TrafficEscape.Services;
namespace TrafficEscape.Pages;

public partial class GamePage : ContentPage
{
    private readonly PlayerCar playerCar = new();
    private readonly LaneManager laneManager = new();

    private readonly List<EnemyCar> enemies = new();
    private readonly List<PickupCoin> pickups = new();
    private readonly DifficultyService difficulty = new();

    private GameLoop? gameLoop;

    private bool isMoving;
    private double score;
    private Spawner spawner;
    private double spawnTimer;
    private const double ScoreRate = 1.5;
    private double spawnInterval = 2.8;
    private const int MaxEnemies = 3;
    private const int MaxPickups = 2;
    private int lastSpawnLane = -1;
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

        spawner = new Spawner(difficulty);

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
    }
    private void UpdateGame(double update)
    {
        //spawning
        spawnTimer += update;

        if (spawnTimer > spawnInterval)
        {
            spawnTimer = 0;

            SpawnRandom();
        }

        //score
        score += update * ScoreRate;
        ScoreLabel.Text = ((int)score).ToString();

        //difficulty scaling 
        spawnInterval = Math.Max(1.2, spawnInterval - update * 0.002);

        UpdateEnemies(update);
        UpdatePickups(update);
    }
    private bool IsLaneBlocked(int lane)
    {
        const double spawnBlockDistance = 300;

        foreach (var e in enemies)
        {
            if (e.Lane == lane && e.Y < spawnBlockDistance)
                return true;
        }

        return false;
    }
    private void SpawnRandom()
    {
        var freeLanes = new List<int>();

        for (int lane = 0; lane < 3; lane++)
        {
            if (!IsLaneBlocked(lane))
                freeLanes.Add(lane);
        }

        if (freeLanes.Count <= 1)
            return;

        int laneToUse = freeLanes[Random.Shared.Next(freeLanes.Count)];

        if (Random.Shared.NextDouble() < 0.65)
        {
            if (enemies.Count < MaxEnemies)
                SpawnEnemy(laneToUse);
        }
        else
        {
            if (pickups.Count < MaxPickups)
                SpawnPickup(laneToUse);
        }
    }
    private void SpawnEnemy(int lane)
    {
        var enemy = spawner.CreateEnemy(lane);

        var img = new Image
        {
            Source = "obstacle.png",
            WidthRequest = 80,
            HeightRequest = 160
        };

        enemy.View = img;
        enemies.Add(enemy);

        ObstacleLayer.Children.Add(img);
    }
    private void UpdateEnemies(double delta)
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            var e = enemies[i];
            e.Y += e.Speed * delta;

            PositionInLane(e.View, e.Lane, e.Y);

            if (CheckCollision(e.View))
            {
                GameOver();
                return;
            }

            if (e.Y > Height + 200)
            {
                ObstacleLayer.Children.Remove(e.View);
                enemies.RemoveAt(i);
            }
        }
    }
    private void SpawnPickup(int lane)
    {
        var coin = spawner.CreatePickup(lane);

        var img = new Image
        {
            Source = "coin.png",
            WidthRequest = 50,
            HeightRequest = 50
        };

        coin.View = img;
        pickups.Add(coin);

        PickupLayer.Children.Add(img);
    }
    private void UpdatePickups(double delta)
    {
        for (int i = pickups.Count - 1; i >= 0; i--)
        {
            var p = pickups[i];
            p.Y += p.Speed * delta;

            PositionInLane(p.View, p.Lane, p.Y);

            if (CheckCollision(p.View))
            {
                PickupLayer.Children.Remove(p.View);
                pickups.RemoveAt(i);
                continue;
            }

            if (p.Y > Height + 200)
            {
                PickupLayer.Children.Remove(p.View);
                pickups.RemoveAt(i);
            }
        }
    }

    private void PositionInLane(View view, int lane, double y)
    {
        double x = laneManager.LanePositions[lane];

        AbsoluteLayout.SetLayoutBounds(
            view,
            new Rect(
                x - view.WidthRequest / 2,
                y,
                view.WidthRequest,
                view.HeightRequest
            )
        );
    }
    private bool CheckCollision(View other)
    {
        return PlayerCarView.Bounds.IntersectsWith(other.Bounds);
    }

    private async Task AnimateCarToLane(int lane)
    {

        isMoving = true;

        double targetX =
            laneManager.LanePositions[lane] - (PlayerCarView.Width / 2);

        double deltaX = targetX - PlayerCarView.X;
        await PlayerCarView.TranslateTo(deltaX, 0, 180, Easing.CubicOut);

        AbsoluteLayout.SetLayoutBounds(
        PlayerCarView,
        new Rect(
            targetX,
            PlayerCarView.Y,
            PlayerCarView.Width,
            PlayerCarView.Height
        )
    );

        PlayerCarView.TranslationX = 0;

        isMoving = false;
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

        await Shell.Current.GoToAsync("..");
    }
}
