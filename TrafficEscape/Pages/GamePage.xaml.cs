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
    }
        //==============================
        //LANE POSITIONING 
        //==============================
        private void CalculateLanePositions()
        {
            double width = LaneGrid.width;
            
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

        AbsolouteLayout.SetLayoutBounds(PlayerCar,
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
            var swipeLeft = new SwipeGestureRecognizer { Direction = SwipeDirection.Left } ;
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
            IsMoving = true;
            double targetX = LanePositions[lane] - (PlayerCar.Width / 2);

            await PlayerCar.TranslateTo(targetX - PlayerCar.X, 0, 150, Easing.CubicOut);

            PositionPlayerCar();
            IsMoving = false;
        }

}


