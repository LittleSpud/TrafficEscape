using Plugin.Maui.Audio;
using Microsoft.Maui;
using Microsoft.Extensions.Logging;
using TrafficEscape.Services;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace TrafficEscape
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton(AudioManager.Current);
            //builder.Services.AddSingleton<SaveService>();
            //builder.Services.AddSingleton<SoundService>();
            //builder.Services.AddSingleton<SkinService>();


            //builder.Services.AddSingleton<AudioManager>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
