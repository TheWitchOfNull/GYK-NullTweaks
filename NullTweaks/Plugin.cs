using BepInEx;
using BepInEx.Configuration;
using NullTweaks.Features;
using UnityEngine;

namespace NullTweaks;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static HarmonyLib.Harmony HarmonyInstance;
    public void Awake()
    {
        Log.Initialize(Logger);
        Log.Info($"NullTweaks here, ready to rock.");
        HarmonyInstance = new("GYKMP");

        InstallFeatures();
    }

    private void InstallFeatures()
    {
        string header = "Changing these settings requires restarting the game.";
        var desard = Config.Bind<bool>(header, "Desardify Dialogue", false, "Change \"sard\" to a more explicit alternative (an f-bomb).\nChanging this value requires restarting the game.");
        if (desard.Value)
        {
            Desard.Install();
        }
        var holiday = Config.Bind<HolidayOverride>(header, "Override Holiday State", HolidayOverride.Default, "Forces the game into a specific holiday state.\nChanging this value requires restarting the game.");
        if (holiday.Value != HolidayOverride.Default)
        {
            Holiday.Install(holiday.Value);
        }
    }
}
