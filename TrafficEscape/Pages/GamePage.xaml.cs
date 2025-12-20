using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using TrafficEscape.GameLogic;
using TrafficEscape.Services;
namespace TrafficEscape.Pages;

public partial class GamePage : ContentPage
{
    private readonly PlayerCar playerCar = new();
    private readonly LaneManager laneManager = new();
    private readonly DifficultyService difficulty = new();

    private readonly List<EnemyCar> enemies = new();
    private readonly List<PickupCoin> pickups = new();

    private GameLoop? gameLoop;

    private bool isMoving;
    private double score;

    private Spawner? spawner;
    private double spawnTimer;
    private double difficultyTimer;

    private const double ScoreRate = 0.2;
    private double spawnInterval = 4.0;
    private const double MinSpawnInterval = 1.5;

    private const int MaxEnemies = 3;
    private const int MaxPickups = 2;

    private int sessionCoins = 0;

    public GamePage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        SizeChanged += OnPageSized;
        SoundService.StopMusic();
    }
    private async void OnPageSized(object? sender, EventArgs e)
    {
        SizeChanged -= OnPageSized;
        SoundService.StopMusic();
        await Task.Delay(50);

        laneManager.CalculateLanePositions(LaneGrid.Width);

        playerCar.CurrentLane = 1;
        PositionPlayerCar();

        PlayerCarView.Source = SkinService.EquippedSkin;

        spawner = new Spawner(difficulty);

        gameLoop = new GameLoop(Dispatcher, UpdateGame);
        gameLoop.Start();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        SoundService.PlayMusic();
    }
    private void OnPause(object sender, EventArgs e)
    {
        gameLoop?.Stop();
        PauseOverlay.IsVisible = true;
    }
    private async void OnSettings(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
    private void OnResume(object sender, EventArgs e)
    {
        PauseOverlay.IsVisible = false;
        SoundService.StopMusic();
        gameLoop?.Start();
    }
    private async void OnMenu(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Quit", "Are you sure you want to quit? All progress will be reset!", "Yes", "No");
        if (confirm)
        {
            gameLoop?.Stop();
            await Shell.Current.GoToAsync("..");
        }
    }
    private async void OnQuitToMenu(object sender, EventArgs e)
    {
        SoundService.PlayClick();
        await Shell.Current.GoToAsync("..");
    }

    //player clicks "TRY AGAIN"
    private void OnRestart(object sender, EventArgs e)
    {
        SoundService.PlayClick();
        GameOverOverlay.IsVisible = false;
        ResetGame();
    }
    private void ResetGame()
    {
        score = 0;
        sessionCoins = 0;
        spawnTimer = 0;
        difficultyTimer = 0;
        spawnInterval = 4.0; 

        ScoreLabel.Text = "0";
        CoinLabel.Text = "0";

        foreach (var enemy in enemies)
        {
            ObstacleLayer.Children.Remove(enemy.View);
        }
        enemies.Clear();

        foreach (var coin in pickups)
        {
            PickupLayer.Children.Remove(coin.View);
        }
        pickups.Clear();

        playerCar.CurrentLane = 1;
        PositionPlayerCar();

        gameLoop?.Start();
    }

    private void PositionPlayerCar()
    {

        double x = laneManager.LanePositions[playerCar.CurrentLane];

        AbsoluteLayout.SetLayoutBounds(
        PlayerCarView,
        new Rect(
            x - PlayerCarView.Width / 2,
            Height * 0.75,
            PlayerCarView.WidthRequest,
            PlayerCarView.HeightRequest
            )
        );
    }
    private void UpdateGame(double update)
    {
        //score
        score += update * ScoreRate;

        ScoreLabel.Text = ((int)score).ToString();

        //difficulty
        difficultyTimer += update;

        if (difficultyTimer >= 100)
        {
            difficultyTimer = 0;
            difficulty.IncreaseDifficulty();
        }

        //spawning
        spawnTimer += update;

        if (spawnTimer > spawnInterval)
        {
            spawnTimer = 0;

            SpawnRandom();
        }

        spawnInterval = Math.Max(3.5, spawnInterval - update * 0.005);

        UpdateEnemies(update);
        UpdatePickups(update);
    }
    private void SpawnRandom()
    {
        var blockedLanes = new HashSet<int>();

        foreach (var e in enemies)
        {
            if (e.Y < 250)
                blockedLanes.Add(e.Lane);
        }
        if (blockedLanes.Count >= 2)
            return;

        var availableLanes = Enumerable.Range(0, 3)
        .Where(l => !blockedLanes.Contains(l))
        .ToList();

        if (availableLanes.Count == 0)
            return;

        int lane = availableLanes[Random.Shared.Next(availableLanes.Count)];

        double roll = Random.Shared.NextDouble();

        if (roll < difficulty.PickupChance && pickups.Count < MaxPickups)
        {
            SpawnPickup(lane);
        }
        else if (enemies.Count < MaxEnemies)
        {
            SpawnEnemy(lane);
        }
    }
    private void SpawnEnemy(int lane)
    {
        if (spawner is null)
            throw new InvalidOperationException("Spawner is not initialized.");

        var enemy = spawner.CreateEnemy(lane);

        enemy.View = new Image
        {
            Source = "obstacle.png",
            WidthRequest = 80,
            HeightRequest = 160
        };
        enemies.Add(enemy);

        ObstacleLayer.Children.Add(enemy.View);
    }
    private void UpdateEnemies(double delta)
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            var e = enemies[i];
            double cappedMultiplier = Math.Min(difficulty.SpeedMultiplier, 0.5);
            e.Y += e.Speed * delta * cappedMultiplier;

            PositionInLane(e.View, e.Lane, e.Y);

            if (CheckCollision(e.View))
            {
                if (Preferences.Default.Get("VibrationEnabled", true))
                {
                    HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
                }
                SoundService.PlayCollision();
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
        if (spawner is null)
            throw new InvalidOperationException("Spawner is not initialized.");

        var coin = spawner.CreatePickup(lane);

        coin.View = new Image
        {
            Source = "coin.png",
            WidthRequest = 50,
            HeightRequest = 50
        };
        pickups.Add(coin);

        PickupLayer.Children.Add(coin.View);
    }
    private void UpdatePickups(double delta)
    {
        for (int i = pickups.Count - 1; i >= 0; i--)
        {
            var p = pickups[i];
            double cappedMultiplier = Math.Min(difficulty.SpeedMultiplier, 0.5);
            p.Y += (p.Speed * 0.5) * delta * cappedMultiplier;

            PositionInLane(p.View, p.Lane, p.Y);

            if (CheckCollision(p.View))
            {
                SoundService.PlayCoinSound();
                sessionCoins++;

                CoinLabel.Text = sessionCoins.ToString();

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
    private HashSet<int> GetOccupiedEnemyLanes()
    {
        HashSet<int> lanes = new();

        foreach (var enemy in enemies)
        {
            
            if (enemy.Y < Height * 0.55)
                lanes.Add(enemy.Lane);
        }

        return lanes;
    }
    private bool CheckCollision(View other)
    {
        double playerX = PlayerCarView.X + PlayerCarView.TranslationX;
        double playerY = PlayerCarView.Y + PlayerCarView.TranslationY;

        //hitbox shrinks so there is no unfair collision
        double paddingX = 15;
        double paddingY = 20;

        Rect playerRect = new Rect(
        playerX + paddingX,
        playerY + paddingY,
        PlayerCarView.Width - (paddingX * 2),
        PlayerCarView.Height - (paddingY * 2));

        Rect otherRect = new Rect(
            other.X + paddingX,
            other.Y + paddingY,
            other.WidthRequest - (paddingX * 2),
            other.HeightRequest - (paddingY * 2));

        return playerRect.IntersectsWith(otherRect);
    }

    private async Task AnimateCarToLane(int lane)
    {

        isMoving = true;

        double targetX =
            laneManager.LanePositions[lane] - (PlayerCarView.Width / 2);

        await PlayerCarView.TranslateTo(targetX - PlayerCarView.X, 0, 180, Easing.CubicOut);

        AbsoluteLayout.SetLayoutBounds(
        PlayerCarView,
        new Rect(
            targetX,
            PlayerCarView.Y,
            PlayerCarView.Width,
            PlayerCarView.Height
        )
    );
        PositionPlayerCar();

        PlayerCarView.TranslationX = 0;

        isMoving = false;
    }
    private async void MoveLeft()
    {
        if (isMoving || playerCar.CurrentLane <= 0) return;

        SoundService.PlayTireSound();
        if (Preferences.Default.Get("VibrationEnabled", true))
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        }

        playerCar.CurrentLane--;
        await AnimateCarToLane(playerCar.CurrentLane);
    }

    private async void MoveRight()
    {
        if (isMoving || playerCar.CurrentLane >= 2) return;

        SoundService.PlayTireSound();
        if (Preferences.Default.Get("VibrationEnabled", true))
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        }

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
    private async void GameOver()
    {
        gameLoop?.Stop();

        bool isNewRecord = (int)score > SaveService.HighScore;

        GameOverCoinLabel.Text = sessionCoins.ToString();

        SaveService.Coins += sessionCoins;
        SaveService.HighScore = Math.Max(SaveService.HighScore, (int)score);

        FinalScoreLabel.Text = ((int)score).ToString();
        GameOverHighScoreLabel.Text = SaveService.HighScore.ToString();

        NewRecordLabel.IsVisible = isNewRecord;
        GameOverOverlay.IsVisible = true;

    }
}
