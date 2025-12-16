using System.Windows.Input;
using TrafficEscape.Pages;

namespace TrafficEscape.ViewModels;

public class MainMenuViewModel
{
    public ICommand PlayCommand { get; }
    public ICommand SettingsCommand { get; }
    public ICommand ShopCommand { get; }

    public MainMenuViewModel()
    {
        PlayCommand = new Command(async () =>
            await Shell.Current.GoToAsync(nameof(GamePage)));

        SettingsCommand = new Command(async () =>
            await Shell.Current.GoToAsync(nameof(SettingsPage)));

        ShopCommand = new Command(async() => 
            await Shell.Current.GoToAsync(nameof(ShopPage)));
    }
}

