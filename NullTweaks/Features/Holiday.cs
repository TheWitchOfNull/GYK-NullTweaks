using HarmonyLib;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace NullTweaks.Features;

public static class Holiday
{
    private static HolidayOverride _choice;
    public static void Install(HolidayOverride choice)
    {
        Log.Info("Installing feature: holiday override");
        _choice = choice;
        var h = Plugin.HarmonyInstance;
        var og = AccessTools.Method(typeof(GameSave), nameof(GameSave.GlobalEventsCheck));
        var pf = new HarmonyMethod(typeof(Holiday).GetMethod(nameof(GameSave_GlobalEventsCheck_Prefix)));
        h.Patch(og, prefix: pf);
    }

    public static bool GameSave_GlobalEventsCheck_Prefix()
    {
        if (_choice == HolidayOverride.Default) return true;
        if (_choice == HolidayOverride.ForceNormal)
        {
            GlobalEventBase halloween = new GlobalEventBase("halloween", DateTime.MinValue, TimeSpan.MaxValue)
            {
                on_start_script = new Scene1100_To_SceneHelloween(),
                on_finish_script = new SceneHelloween_To_Scene1100()
            };
            if (MainGame.me.player.GetParamInt(halloween.game_res) == 1)
            {
                AccessTools.Method(typeof(GlobalEventBase), "OnFinishEvent").Invoke(halloween, []);
            }
            return false;
        }
        if(_choice == HolidayOverride.ForceHalloween)
        {
            GlobalEventBase halloween = new GlobalEventBase("halloween", DateTime.MinValue, TimeSpan.MaxValue)
            {
                on_start_script = new Scene1100_To_SceneHelloween(),
                on_finish_script = new SceneHelloween_To_Scene1100()
            };
            if(MainGame.me.player.GetParamInt(halloween.game_res) == 0)
            {
                AccessTools.Method(typeof(GlobalEventBase), "OnStartEvent").Invoke(halloween, []);
            }
        }
        return false;
    }
}

public enum HolidayOverride
{
    [Description("Default - Let the game decide.")]
    Default,
    [Description("Force Normal - Don't want any holidays here.")]
    ForceNormal,
    [Description("Force Halloween - Perma-spoopy mode.")]
    ForceHalloween
}