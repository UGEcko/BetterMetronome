using HarmonyLib;
using UnityEngine;

namespace BetterMetronome.Patches;

[HarmonyPatch(typeof(MetronomeHandler), nameof(MetronomeHandler.LateUpdate))]
public class LateUpdate()
{
    [HarmonyPrefix]
    public static bool Prefix(MetronomeHandler __instance)
    {
        var i = __instance;
        
        
        if (Plugin.metronomeActive && i.atsc.IsPlaying && !i.atsc.StopScheduled)
        {
            if (i.atsc.CurrentAudioBeats > i.queuedDingSongBpmTime)
            {
                var nextJsonTime = Mathf.Ceil(i.atsc.CurrentJsonTime);
                
                if (Mathf.Abs(Mathf.Floor(i.bpmChangeGridContainer.SongBpmTimeToJsonTime(i.atsc.CurrentAudioBeats))
                              - Mathf.Floor(i.atsc.CurrentJsonTime)) > 0.01f)
                {
                    nextJsonTime = Mathf.Ceil(nextJsonTime + 1f);
                }
                i.queuedDingSongBpmTime = i.bpmChangeGridContainer.JsonTimeToSongBpmTime(nextJsonTime);

                var delay = i.atsc.GetSecondsFromBeat(i.queuedDingSongBpmTime - i.atsc.CurrentAudioBeats) / i.songSpeed;
                i.audioUtil.PlayOneShotSound(i.CowBell ? i.cowbellSound : i.metronomeSound, Settings.Instance.MetronomeVolume,
                    1f, delay);

                if (!i.metronomeUI.activeInHierarchy) i.metronomeUI.SetActive(true);
                i.RunAnimation(60f / delay);
            }
        }
        else
        {
            i.metronomeUI.SetActive(false);
        }

        return false;
    }
}