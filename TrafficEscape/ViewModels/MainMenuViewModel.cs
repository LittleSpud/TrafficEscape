using System;
using System.Threading.Tasks;
using System.Windows.Input;
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
        PlayCommand = new Command(async () => await GoToGame());
        SettingsCommand = new Command(async () => await GoToSettings());
        ShopCommand = new Command(async () => await GoToShop());
        QuitCommand = new Command(QuitGame);
    }

    private INavigation? GetNavigation()
    {
        var app = Application.Current;
        if (app != null && app.Windows.Count > 0)
        {
            var mainPage = app.Windows[0].Page;
            return mainPage?.Navigation;
        }
        return null;
    }

    private async Task GoToGame()
    {
        var navigation = GetNavigation();
        if (navigation != null)
        {
            await navigation.PushAsync(new GamePage());
        }
    }

    private async Task GoToSettings()
    {
        var navigation = GetNavigation();
        if (navigation != null)
        {
            await navigation.PushAsync(new SettingsPage());
        }
    }

    private async Task GoToShop()
    {
        var navigation = GetNavigation();
        if (navigation != null)
        {
            await navigation.PushAsync(new ShopPage());
        }
    }

    private void QuitGame()
    {
        //only works on windows/android
        // Implement quit logic here
        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
    }
}

