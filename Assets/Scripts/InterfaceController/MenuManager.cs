using UnityEngine;

public enum MenuStates
{
    MainMenu = 0,
    AudioSettings = 1,
    InputSettings = 2
}

public class MenuManager : MonoBehaviour
{

    [SerializeField] GameObject _menu;

    GameObject _mainCanvas;
    GameObject _audioCanvas;
    GameObject _inputCanvas;

    AudioSettings _audioSettings;
    InputSettings _inputSettings;

    private void Awake()
    {
        _mainCanvas = _menu.transform.GetChild(0).gameObject;
        _audioCanvas = _menu.transform.GetChild(1).gameObject;
        _inputCanvas = _menu.transform.GetChild(2).gameObject;

        _audioSettings = _audioCanvas.GetComponent<AudioSettings>();
        _inputSettings = _inputCanvas.GetComponent<InputSettings>();
    }

    public void ChangeCanvas(int i)
    {
        switch(i)
        {
            case (int)MenuStates.MainMenu:
                _mainCanvas.SetActive(true);
                _audioCanvas.SetActive(false);
                _inputCanvas.SetActive(false);
                break;
            case (int)MenuStates.AudioSettings:
                _mainCanvas.SetActive(false);
                _audioCanvas.SetActive(true);
                _inputCanvas.SetActive(false);
                break;
            case (int)MenuStates.InputSettings:
                _mainCanvas.SetActive(false);
                _audioCanvas.SetActive(false);
                _inputCanvas.SetActive(true);
                break;
        }
    }

    public void StoreValues() 
    {
        StaticData.MasterVolume = _audioSettings.GetMasterVolume();
        StaticData.MusicVolume = _audioSettings.GetMusicVolume();
        StaticData.AmbienceVolume = _audioSettings.GetAmbienceVolume();
        StaticData.EffectsVolume = _audioSettings.GetEffectsVolume();
        StaticData.VoicesVolume = _audioSettings.GetVoicesVolume();
        StaticData.InteractHelp = _inputSettings.GetToggle();
    }
}
