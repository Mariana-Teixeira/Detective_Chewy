using UnityEngine;

public enum MenuStates
{
    MainMenu = 0,
    AudioSettings = 1,
    InputSettings = 2
}

public class MenuManager : MonoBehaviour
{
    public GameObject MenuComponent;
    public MenuCanvasScript MenuCanvas;

    GameObject _audioComponent;
    GameObject _inputComponent;

    AudioSettings _audioSettings;
    InputSettings _inputSettings;

    GameState _previousState;
    bool _isVisible;

    public bool ListenForToggleMenu
    {
        get
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void Awake()
    {
        _audioComponent = MenuComponent.transform.GetChild(1).gameObject;
        _inputComponent = MenuComponent.transform.GetChild(2).gameObject;

        _audioSettings = _audioComponent.GetComponent<AudioSettings>();
        _inputSettings = _inputComponent.GetComponent<InputSettings>();
    }

    public void ToggleCanvas(GameState state = GameState.NULL)
    {
        if (_isVisible)
        {
            _isVisible = false;
            MenuCanvas.DisableCanvas(_previousState);
        }
        else
        {
            _isVisible = true;
            _previousState = state;
            MenuCanvas.EnableCanvas();
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
