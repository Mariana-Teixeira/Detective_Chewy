using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static float MasterVolume = 100;
    public static float MusicVolume = 100;
    public static float AmbienceVolume = 100;
    public static float EffectsVolume = 100;
    public static float VoicesVolume = 100;
    public static string InteractBtn = "";


    private void Start()
    {
        MasterVolume = 100;
        MusicVolume = 100;
        AmbienceVolume = 100;
        EffectsVolume = 100;
        VoicesVolume = 100;
        InteractBtn = "";

    }
}
