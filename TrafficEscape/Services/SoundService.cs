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
    private static IAudioPlayer? collisionPlayer;
        private static bool initialised;

        public static async Task InitAsync(IAudioManager audioManager)
        {
            if (initialised) return;

            try
            {
                var clickStream = await FileSystem.OpenAppPackageFileAsync("click.wav");
                clickPlayer = audioManager.CreatePlayer(clickStream);

                var crashStream = await FileSystem.OpenAppPackageFileAsync("collision.wav");
                collisionPlayer = audioManager.CreatePlayer(crashStream);

                initialised = true;
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
        public static void PlayCollision()
        {
            if(collisionPlayer == null) 
               return;
            if(collisionPlayer.IsPlaying)
               collisionPlayer.Stop();

            collisionPlayer?.Play();
        }
}

