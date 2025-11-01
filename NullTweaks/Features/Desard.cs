using HarmonyLib;

namespace NullTweaks.Features;

public static class Desard
{
    public static void Install()
    {
        Log.Info("Installing feature: desardification");
        var h = Plugin.HarmonyInstance;
        var og = AccessTools.Method(typeof(LocalizedLabel), nameof(LocalizedLabel.ColorizeTags), [typeof(string), typeof(LocalizedLabel.TextColor)]);
        var pf = new HarmonyMethod(typeof(Desard).GetMethod(nameof(LocalizedLabel_ColorizedTag_Postfix)));
        h.Patch(og, postfix: pf);
    }

    public static void LocalizedLabel_ColorizedTag_Postfix(ref string __result)
    {
        if (__result.ToLowerInvariant().Contains("sard"))
        {
            Log.Info("Desardifying response.");
            __result = __result.Replace("sard", "fuck").Replace("Sard", "Fuck").Replace("SARD", "FUCK");
        }
    }
}