using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsScript : MonoBehaviour
{
    [SerializeField] Slider sliderVolume;
    [SerializeField] Slider sliderSubtitles;


    private void Start()
    {
        sliderVolume.value = StaticData.Volume;
        sliderSubtitles.value = StaticData.Subtitles;
    }
    public float GetVolume() 
    {
        return sliderVolume.value;
    }

    public float GetSubtitles()
    {
        return sliderSubtitles.value;
    }
}
