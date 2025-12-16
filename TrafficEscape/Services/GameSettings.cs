using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace TrafficEscape.Services;

public static class GameSettings
{
    private const string DarkModeKey = "DarkMode";

    public static bool UserDarkMode
    {
        get => Preferences.Get(DarkModeKey, false);
        set => Preferences.Set(DarkModeKey, value);
    }
}
