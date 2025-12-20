using Plugin.Maui.Audio;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TrafficEscape.Services;

    public static class SoundService
    {
     
        private static IAudioPlayer? clickPlayer;
        private static bool clicked;

        public static async Task InitAsync(IAudioManager audioManager)
        {
            if (clicked) return;

            try
            {
                var stream = await FileSystem.OpenAppPackageFileAsync("click.wav");
                clickPlayer = audioManager.CreatePlayer(stream);
                clicked = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Audio init failed: {ex}");
            }
        }

        public static void PlayClick()
        {
            if (clickPlayer == null)
                return;
            if (clickPlayer.IsPlaying)
                clickPlayer.Stop();

            clickPlayer?.Play();
        }
    }

