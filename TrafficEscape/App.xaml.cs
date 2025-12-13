using TrafficEscape.Services;

namespace TrafficEscape;
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Apply saved theme
            App.Current.UserAppTheme =
                GameSettings.UserDarkMode ? AppTheme.Dark : AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }