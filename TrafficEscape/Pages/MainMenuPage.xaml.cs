using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficEscape.Pages
{
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        async void StartGame(object sender, EventArgs e)
            => await Navigation.PushAsync(new GamePage());

        async void OpenShop(object sender, EventArgs e)
            => await Navigation.PushAsync(new ShopPage());

        async void HighScores(object sender, EventArgs e)
            => await Navigation.PushAsync(new HighScoresPage());

        async void OpenSettings(object sender, EventArgs e)
            => await Navigation.PushAsync(new SettingsPage());

    }
}
