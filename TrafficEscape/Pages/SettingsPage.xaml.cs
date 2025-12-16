using TrafficEscape.Services;

namespace TrafficEscape.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        DarkModeSwitch.IsToggled = GameSettings.UserDarkMode;
    }

    private void OnDarkModeToggled(object sender, ToggledEventArgs e)
    {
        GameSettings.UserDarkMode = e.Value;
        Application.Current.UserAppTheme =
            e.Value ? AppTheme.Dark : AppTheme.Light;
    }
}