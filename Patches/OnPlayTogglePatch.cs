using HarmonyLib;
using UnityEngine;

namespace BetterMetronome.Patches;

[HarmonyPatch(typeof(MetronomeHandler), nameof(MetronomeHandler.OnPlayToggle))]
public class OnPlayTogglePatch()
{
    [HarmonyPrefix]
    public static bool Prefix3(MetronomeHandler __instance, ref bool playing)
    {
        var i = __instance;
        
        if (!Plugin.metronomeActive)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}