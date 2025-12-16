using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using Plugin.Maui.Audio;
using TrafficEscape.Services;
namespace TrafficEscape;

    public partial class App : Application
    {
        public App(IAudioManager audioManager)
        {
            InitializeComponent();
        _ = SoundService.InitAsync(audioManager);

        // Apply saved theme
        App.Current.UserAppTheme =
                GameSettings.UserDarkMode ? AppTheme.Dark : AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }