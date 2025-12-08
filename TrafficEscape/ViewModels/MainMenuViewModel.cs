using System;
using System.Windows.Input;
using TrafficEscape.Pages;

namespace TrafficEscape.ViewModels;

public class MainMenuViewModel : BindableObject
{
    public ICommand PlayCommand { get; }
    public ICommand SettingsCommand { get; }
    public ICommand ShopCommand { get; }
    public ICommand QuitCommand { get; }

}
    public MainMenuViewModel()
    {
        PlayCommand = new Command(async () => await GoToGame());
        SettingsCommand = new Command(async () => await GoToSettings());
        ShopCommand = new Command(async () => await GoToShop());
        QuitCommand = new Command(QuitGame);
    }
    private async Task GoToGame()
    {
        await Application.Current.MainPage.Navigation.PushAsync(new GamePage());
    }
    private async Task GoToSettings()
    {
        await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
    }
    private async Task GoToShop()
    {
        await Application.Current.MainPage.Navigation.PushAsync(new ShopPage());
    }
    private void QuitGame()
    {
        //only works on windows/android
        // Implement quit logic here
        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
    }
}

