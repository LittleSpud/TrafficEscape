using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficEscape.Services;
    public static class SkinService
    {
    public static List<string> AvailableSkins = new()
    {
        "car_1.png",
        "car_2.png",
        "car_3.png",
        "car_4.png",
        "car_5.png",
        "car_6.png",
        "car7.png"
    };

    public const int SkinPrice = 50;

    public static string EquippedSkin
    {
        get => Preferences.Get("EquippedSkin", "car_default.png");
        set => Preferences.Set("EquippedSkin", value);
    }
    public static bool IsSkinOwned(string skinName)
    {
        if (skinName == "car_default.png") return true;
        return Preferences.Get($"Owned_{skinName}", false);
    }

    public static void SetSkinOwned(string skinName)
    {
        Preferences.Set($"Owned_{skinName}", true);
    }
}

