using HarmonyLib;
using UnityEngine;

namespace BetterMetronome.Patches;


[HarmonyPatch(typeof(MetronomeHandler), nameof(MetronomeHandler.Start))]
public class StartPatch
{
    [HarmonyPrefix]
    public static bool Prefix(MetronomeHandler __instance)
    {
        var i = __instance;
        i.metronomeUIAnimator = i.metronomeUI.GetComponent<Animator>();
        Plugin.metronomeActive = Settings.Instance.MetronomeVolume != 0;

        Settings.NotifyBySettingName("SongSpeed", i.UpdateSongSpeed);
        Settings.NotifyBySettingName("MetronomeVolume", value =>
        {
            if ((float)value != 0 && !Plugin.metronomeActive)
            {
                Plugin.metronomeActive = true;
            }
            else if (Plugin.metronomeActive && (float)value == 0)
            {
                Plugin.metronomeActive = false;
            }
        });

        i.atsc.PlayToggle += i.OnPlayToggle;

        return false;
    }
}



