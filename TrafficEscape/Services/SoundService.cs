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
     
    private static IAudioPlayer? clickPlayer;
    private static IAudioPlayer? collisionPlayer;
    private static IAudioPlayer? menuMusicPlayer;
    private static IAudioPlayer? tireSoundPlayer;
    private static IAudioPlayer? coinPlayer;
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

                var musicStream = await FileSystem.OpenAppPackageFileAsync("menu_music.wav");
                menuMusicPlayer = audioManager.CreatePlayer(musicStream);
                menuMusicPlayer.Loop = true;
                menuMusicPlayer.Volume = Preferences.Default.Get("MusicVolume", 0.5);

                var tireStream = await FileSystem.OpenAppPackageFileAsync("movement.mp3");
                tireSoundPlayer = audioManager.CreatePlayer(tireStream);

                var coinStream = await FileSystem.OpenAppPackageFileAsync("coin.mp3");
                coinPlayer = audioManager.CreatePlayer(coinStream);

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
        public static void PlayMusic()
        {
            if (menuMusicPlayer == null || menuMusicPlayer.IsPlaying) return;
           
                menuMusicPlayer.Play();
        }
        public static void SetMusicVolume(double volume)
        {
            if (menuMusicPlayer != null)
            {
                menuMusicPlayer.Volume = volume;
            }
        }
        public static void StopMusic()
        {
            try
            {
                if (menuMusicPlayer != null)
                {
                    if (menuMusicPlayer.IsPlaying)
                    {
                        menuMusicPlayer.Stop();
                    }
                        menuMusicPlayer.Volume = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"StopMusic failed: {ex.Message}");
            }
        }
        public static void PlayTireSound()
        {
            if (tireSoundPlayer == null) return;

            bool soundEnabled = Preferences.Default.Get("SoundEnabled", true);

            if (soundEnabled)
            {
                if (tireSoundPlayer.IsPlaying)
                    tireSoundPlayer.Stop();

                tireSoundPlayer.Volume = Preferences.Default.Get("SFXVolume", 0.5);
                tireSoundPlayer.Play();
            }
        }
        public static void PlayCoinSound()
        {
            if (coinPlayer == null) return;

            bool soundEnabled = Preferences.Default.Get("SoundEnabled", true);
            if (soundEnabled)
            {
                if (coinPlayer.IsPlaying)
                    coinPlayer.Stop();

                coinPlayer.Volume = Preferences.Default.Get("SFXVolume", 0.5);
                coinPlayer.Play();
            }
        }
    public static void UpdateSfxVolume(double volume)
        {
            if (clickPlayer != null)
                clickPlayer.Volume = volume;

            if (tireSoundPlayer != null)
                tireSoundPlayer.Volume = volume;

            if (collisionPlayer != null)
                collisionPlayer.Volume = volume;

            if (coinPlayer != null) 
                coinPlayer.Volume = volume;
    }
}

