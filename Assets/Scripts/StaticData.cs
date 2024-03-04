using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static float Volume;
    public static float Subtitles;

    private void Start()
    {
        Volume = 100;
        Subtitles = 1;
    }

}
