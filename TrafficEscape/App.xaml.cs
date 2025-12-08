using TrafficEscape.Services;
using TrafficEscape.Pages;

namespace TrafficEscape
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            App.Current.UserAppTheme = GameSettings.UserDarkMode ? AppTheme.Dark : AppTheme.Light;
            MainPage = new NavigationPage(new MainMenuPage());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}