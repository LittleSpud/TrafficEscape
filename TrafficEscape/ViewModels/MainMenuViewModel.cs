using System.Windows.Input;
using TrafficEscape.Services;
using TrafficEscape.Pages;

namespace TrafficEscape.ViewModels;

public class MainMenuViewModel : BindableObject
{
    public ICommand PlayCommand { get; }
    public ICommand SettingsCommand { get; }
    public ICommand ShopCommand { get; }
    public ICommand QuitCommand { get; }

    public MainMenuViewModel()
    {

        PlayCommand = new Command(async () =>
        {
            SoundService.PlayClick();
            await Shell.Current.GoToAsync(nameof(GamePage));
        });

        SettingsCommand = new Command(async () =>
        {
            SoundService.PlayClick();
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        });

        ShopCommand = new Command(async () =>
        {
            SoundService.PlayClick();
            await Shell.Current.GoToAsync(nameof(ShopPage));
        });

        QuitCommand = new Command(() =>
        {
            SoundService.PlayClick();
#if WINDOWS
            Application.Current?.Quit();
#endif
        });
    }
}

