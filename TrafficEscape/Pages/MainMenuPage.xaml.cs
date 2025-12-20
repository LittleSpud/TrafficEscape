using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficEscape.ViewModels;
using TrafficEscape.Services;

namespace TrafficEscape.Pages;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage()
    {
        InitializeComponent();
        BindingContext = new MainMenuViewModel();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //display/store highscore
        int high = Preferences.Default.Get("HighScore", 0);
        BestScoreLabel.Text = high.ToString();

        //update total coin from save service
        TotalCoinLabel.Text = SaveService.Coins.ToString();

        // Title fade-in
        await TitleLabel.FadeTo(1, 1200, Easing.CubicOut);
        //menu music
        SoundService.PlayMusic();

    }
    private async void OnButtonPressed(object sender, EventArgs e)
    {
        if (sender is not Button btn) return;

        SoundService.PlayClick();

        await btn.ScaleTo(0.9, 80);
        await btn.ScaleTo(1.0, 80);
    }
    private async void OnStart(object sender, EventArgs e)
    {
        SoundService.PlayClick();
        await Shell.Current.GoToAsync(nameof(GamePage));
    }
    private async void OnSettings(object sender, EventArgs e)
    {
        SoundService.PlayClick();
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
    private async void OnGarage(object sender, EventArgs e)
    {
        SoundService.PlayClick();
        await Shell.Current.GoToAsync(nameof(GaragePage));
    }
    private async void OnQuit(object sender, EventArgs e)
    {
        SoundService.PlayClick();
        await Shell.Current.GoToAsync("..");
    }
    private async void OnShop(object sender, EventArgs e)
    {
        SoundService.PlayClick();
        await Shell.Current.GoToAsync(nameof(ShopPage));
    }
    
}
