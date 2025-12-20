using TrafficEscape.Services;

namespace TrafficEscape.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        DarkModeSwitch.IsToggled = GameSettings.UserDarkMode;
        VibrationSwitch.IsToggled = Preferences.Default.Get("VibrationEnabled", true);

        double savedVolume = Preferences.Default.Get("MusicVolume", 0.5);
        double savedSfxVolume = Preferences.Default.Get("SFXVolume", 0.5);

        SfxVolumeSlider.Value = savedSfxVolume;
        MusicVolumeSlider.Value = savedVolume;

        SfxVolumeLabel.Text = $"{savedSfxVolume:P0}";
        VolumeLabel.Text = $"{(int)(savedVolume * 100)}%";
    }

    private void OnDarkModeToggled(object sender, ToggledEventArgs e)
    {
        GameSettings.UserDarkMode = e.Value;
        if (Application.Current != null)
        {
            Application.Current.UserAppTheme =
                e.Value ? AppTheme.Dark : AppTheme.Light;
        }
    }
    private void OnVibrationToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Default.Set("VibrationEnabled", e.Value);
    }
    private void OnVolumeChanged(object sender, ValueChangedEventArgs e)
    {
        Preferences.Default.Set("MusicVolume", e.NewValue);

        VolumeLabel.Text = $"{(int)(e.NewValue * 100)}%";

        SoundService.SetMusicVolume(e.NewValue);
    }
    private void OnSfxVolumeChanged(object sender, ValueChangedEventArgs e)
    {
        SfxVolumeLabel.Text = $"{e.NewValue:P0}";

        Preferences.Default.Set("SFXVolume", e.NewValue);

        SoundService.UpdateSfxVolume(e.NewValue);

        SoundService.PlayClick();
    }
    private async void OnWipeDataClicked(object sender, EventArgs e)
    {
        SoundService.PlayClick();

        bool confirm = await DisplayAlert(
            "Wipe All Data?",
            "Are you absolutely sure? This cannot be undone. You will lose all your coins and scores.",
            "RESET EVERYTHING",
            "Cancel");

        if (confirm)
        {
            Preferences.Default.Clear();

            SaveService.HighScore = 0;
            SaveService.Coins = 0;
    
            SfxVolumeSlider.Value = 0.5;
            MusicVolumeSlider.Value = 0.5;
            SfxVolumeLabel.Text = "50%";
            VolumeLabel.Text = "50%";

            await DisplayAlert("Reset Successful", "Your data has been wiped.", "OK");
        }
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

}