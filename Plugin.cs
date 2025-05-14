using System;
using HarmonyLib;
using UnityEngine;

namespace BetterMetronome;

[Plugin("Better Metronome")]
public class Plugin
{
    private Harmony _harmony;
    public static bool metronomeActive;
    [Init]
    public void Init()
    {
        _harmony = new Harmony("com.ecko.bettermetronome");
        _harmony.PatchAll();
        Debug.Log("Successfully loaded Better Metronome plugin");
    }
    
    [Exit]
    public void Exit() => _harmony.UnpatchSelf();
}