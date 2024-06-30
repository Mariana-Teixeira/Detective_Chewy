using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] Slider sliderMasterVolume;
    [SerializeField] Slider sliderMusicVolume;
    [SerializeField] Slider sliderAmbienceVolume;
    [SerializeField] Slider sliderEffectsVolume;
    [SerializeField] Slider sliderVoicesVolume;


    private void Start()
    {
        sliderMasterVolume.value = StaticData.MasterVolume;
        sliderMusicVolume.value = StaticData.MusicVolume;
        sliderAmbienceVolume.value = StaticData.AmbienceVolume;
        sliderEffectsVolume.value = StaticData.EffectsVolume;
        sliderVoicesVolume.value = StaticData.VoicesVolume;
    }

    public float GetMasterVolume()
    {
        return sliderMasterVolume.value;
    }

    public float GetMusicVolume()
    {
        return sliderMusicVolume.value;
    }

    public float GetAmbienceVolume()
    {
        return sliderAmbienceVolume.value;
    }

    public float GetEffectsVolume()
    {
        return sliderEffectsVolume.value;
    }

    public float GetVoicesVolume()
    {
        return sliderVoicesVolume.value;
    }
}
