using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using TrafficEscape.Pages;

namespace TrafficEscape;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
        Routing.RegisterRoute(nameof(PausePage), typeof(PausePage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(ShopPage), typeof(ShopPage));
    }
}
