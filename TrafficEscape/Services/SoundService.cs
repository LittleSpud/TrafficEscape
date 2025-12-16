
using Microsoft.Maui.Storage;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TrafficEscape.Services;

    public static class SoundService
    {
        //private static IAudioManager? audioManager;
        private static IAudioPlayer? clickPlayer;

        public static async Task InitAsync(IAudioManager manager)
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("click.wav");
            clickPlayer = manager.CreatePlayer(stream);
        }
        public static void PlayClick()
        {
            if (clickPlayer == null)
                return;

            clickPlayer?.Stop();
            clickPlayer?.Play();
        }
    }

