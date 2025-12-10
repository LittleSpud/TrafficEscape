using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using TrafficEscape.Models;

namespace TrafficEscape.Pages;

public partial class GamePage : ContentPage
{
    private Car PlayerCarModel = new Car();
    private double[] LanePositions = new double[3];

    //GAame State
    private bool IsGameRunning = false;
    private double Score = 0;
    private int CoinCount = 0;
    private double GameSpeed = 4;//base speed
    private double SpawnTimer = 0;

    private readonly Random rng = new();

    //moving objects
    private readonly List<Obstacle> Obstacles = new();
    private readonly List<Pickup> Pickups = new();

    public GamePage()
    {
        InitializeComponent();
        SetupInput();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CalculateLanePositions();
        PositionPlayerCar();
        StartGameLoop();
        StartSpawnLoop();
    }
    //==============================
    //LANE POSITIONING 
    //==============================
    private void CalculateLanePositions()
    {
        double width = LaneGrid.Width;

        if (width <= 0)
        {
            LaneGrid.SizeChanged += (s, e) => CalculateLanePositions();
            return;
        }
        double laneWidth = width / 3;
        LanePositions[0] = laneWidth / 0.5; // Left lane center
        LanePositions[1] = laneWidth * 1.5; // Middle lane center
        LanePositions[2] = laneWidth * 2.5; // Right lane center
    }

    private void PositionPlayerCar()
    {
        double x = LanePositions[PlayerCarModel.CurrentLane];

        AbsoluteLayout.SetLayoutBounds(PlayerCar,
            new Rect(x - (PlayerCar.Width / 2),
                     this.Height * 0.75,
                     PlayerCarWidth,
                     PlayerCarHeight));
    }

    //==============================
    //INPUT HANDLING 
    //==============================
    private void SetupInput()
    {
        //Left Swipee 
        var swipeLeft = new SwipeGestureRecognizer { Direction = SwipeDirection.Left };
        swipeLeft.Swiped += (s, e) => MoveLeft();

        //Right Swipee 
        var swipeRight = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };
        swipeRight.Swiped += (s, e) => MoveRight();

        // Tap (decide left/right bassed on position)
        var tap = new TapGestureRecognizer();
        tap.Tapped += OnScreenTapped;

        TouchPad.GestureRecognizers.Add(swipeLeft);
        TouchPad.GestureRecognizers.Add(swipeRight);
        TouchPad.GestureRecognizers.Add(tap);
    }

    private void OnScreenTapped(object sender, EventArgs e)
    {
        double x = e.GetPosition(TouchPad).Value.X;

        if (x < TouchPad.Width / 2)
            MoveLeft();
        else
            MoveRight();
    }

    //==============================
    //Player Movement
    //==============================
    private bool isMoving = false;
    private async void MoveLeft()
    {
        if (isMoving) return;
        if (PlayerCarModel.CurrentLane <= 0) return;

        PlayerCarModel.CurrentLane--;
        await AnimateCarToLane(PlayerCarModel.CurrentLane);
    }

    private async void MoveRight()
    {
        if (isMoving) return;
        if (PlayerCarModel.CurrentLane >= 2) return;

        PlayerCarModel.CurrentLane++;
        await AnimateCarToLane(PlayerCarModel.CurrentLane);
    }

    private async Task AnimateCarToLane(int lane)
    {
        isMoving = true;
        double targetX = LanePositions[lane] - (PlayerCar.Width / 2);

        await PlayerCar.TranslateTo(targetX - PlayerCar.X, 0, 150, Easing.CubicOut);

        PositionPlayerCar();
        isMoving = false;
    }

    //================================
    //Game loop
    //================================
    private void StartGameLoop()
    {
        IsGameRunning = true;

        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(16), () =>
        {
            if (!IsGameRunning) return false;
            UpdateGame(0.016);
            return true;
        });
    }

    private void UpdateGame(double delta)
    {
        //increase player score
        Score += delta * 10;
        ScoreLabel.Text = ((int)Score).ToString();

        //obstacles and pickups
        UpdateObstacles(delta);
        UpdatePickups(delta);

        //difficulty increases over time/game speeds up (Like subway surfers)
        GameSpeed += delta * 0.1;
    }
    //================================
    //Obstacles/enemies
    //================================
    private void StartSpawnLoop()
    {
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(500), () =>
        {
            if (!IsGameRunning) return false;

            SpawnRandom();
            return true;
        });
    }

    private void SpawnRandom()
    {
        int lane = rng.Next(0, 3);
        double roll = rng.NextDouble();

        if (roll < 0.6)
            SpawnObstacle(lane);
        else
            SpawnPickup(lane);
    }

    private void SpawnObstacle(int lane)
    {
        var img = new Image
        {
            Source = "obstacle.png",
            WidthRequest = 80,
            HeightRequest = 160,
        };

        var obj = new Obstacle
        {
            Lane = lane,
            Sprite = img,
            Speed = GameSpeed,
            Y = -200
        };

        Obstacles.Add(obj);

        AbsoluteLayout.SetLayoutBounds(img,
            new Rect(LanePositions[lane] - img.Width / 2, obj.Y, img.Width, img.Height));

        ObstacleLayer.Children.Add(img);
    }

    private void UpdateObstacles(double delta)
    {
        for (int i = Obstacles.Count - 1; i >= 0; i--)
        {
            var o = Obstacles[i];
            o.Y += o.Speed;

            AbsoluteLayout.SetLayoutBounds(o.Sprite,
                new Rect(LanePositions[o.Lane] - o.Sprite.Width / 2,
                         o.Y,
                         o.Sprite.Width,
                         o.Sprite.Height));

            // Collision
            if (CheckCollision(o.Sprite))
            {
                GameOver();
                return;
            }

            // Remove off-screen
            if (o.Y > Height + 200)
            {
                ObstacleLayer.Children.Remove(o.Sprite);
                Obstacles.RemoveAt(i);
            }
        }
    }

    // ==========================================
    // Pickups
    // ==========================================
    private void SpawnPickup(int lane)
    {
        var img = new Image
        {
            Source = rng.NextDouble() < 0.7 ? "coin.png" : "fuel.png",
            WidthRequest = 60,
            HeightRequest = 60,
        };

        var type = img.Source.ToString().Contains("coin") ? PickupType.Coin : PickupType.Fuel;

        var p = new Pickup
        {
            Lane = lane,
            Sprite = img,
            Speed = GameSpeed,
            Type = type,
            Y = -100
        };

        Pickups.Add(p);

        AbsoluteLayout.SetLayoutBounds(img,
            new Rect(LanePositions[lane] - img.Width / 2, p.Y, img.Width, img.Height));

        PickupLayer.Children.Add(img);
    }

    private void UpdatePickups(double delta)
    {
        for (int i = Pickups.Count - 1; i >= 0; i--)
        {
            var p = Pickups[i];
            p.Y += p.Speed;

            AbsoluteLayout.SetLayoutBounds(p.Sprite,
                new Rect(LanePositions[p.Lane] - p.Sprite.Width / 2,
                         p.Y,
                         p.Sprite.Width,
                         p.Sprite.Height));

            // Pickup collision
            if (CheckCollision(p.Sprite))
            {
                if (p.Type == PickupType.Coin)
                {
                    CoinCount++;
                    CoinLabel.Text = CoinCount.ToString();
                }

                PickupLayer.Children.Remove(p.Sprite);
                Pickups.RemoveAt(i);
                continue;
            }

            // Off-screen removal
            if (p.Y > Height + 200)
            {
                PickupLayer.Children.Remove(p.Sprite);
                Pickups.RemoveAt(i);
            }
        }
    }

    // ==========================================
    // COLLISION DETECTION
    // ==========================================
    private bool CheckCollision(View obj)
    {
        Rect p = new Rect(PlayerCar.X, PlayerCar.Y, PlayerCar.Width, PlayerCar.Height);
        Rect o = new Rect(obj.X, obj.Y, obj.Width, obj.Height);

        return p.IntersectsWith(o);
    }

    // ==========================================
    // GAME OVER
    // ==========================================
    private async void GameOver()
    {
        IsGameRunning = false;

        await DisplayAlert("Crash!", $"Score: {(int)Score}", "OK");

        await Navigation.PopAsync();
    }

}


