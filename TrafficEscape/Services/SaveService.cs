using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficEscape.Services;

public static class SaveService
{
    public static int HighScore
    {
        get => Preferences.Get("HighScore", 0);
        set => Preferences.Set("HighScore", value);
    }
    public static int Coins
    {
        get => Preferences.Get("Coins", 0);
        set => Preferences.Set("Coins", value);
    }
}

