using System;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class TutorialCanvasScript : MonoBehaviour
{
    public static Action ClickToNext;
    private Canvas _tutorialCanvas;
    private TMP_Text  _tutorialText;
    private VideoPlayer _videoPlayer;

    public TutorialScreen[] TutorialVideos;
    TutorialScreen _currentTutorial;

    private int TutorialIndex = 0;

    private void Start()
    {
        _tutorialCanvas = GetComponent<Canvas>();
        _videoPlayer = GetComponentInChildren<VideoPlayer>();
        _tutorialText  = GetComponentInChildren<TMP_Text>();
    }

    public void ToggleVisibility(bool isVisible)
    {
        if (isVisible)
        {
            _tutorialCanvas.enabled = true;
            _videoPlayer.Play();
        }
        else
        {
            _tutorialCanvas.enabled = false;
            _videoPlayer.Stop();
        }
    }

    public void SetTutorial()
    {
        _currentTutorial = TutorialVideos[TutorialIndex];
        _tutorialText.text = _currentTutorial.texts[_currentTutorial.index];
        _videoPlayer.clip = _currentTutorial.clips[_currentTutorial.index];
    }

    public void NextTutorialScreen()
    {
        _currentTutorial.index++;

        if (_currentTutorial.index < _currentTutorial.clips.Length)
        {
            _videoPlayer.clip = _currentTutorial.clips[_currentTutorial.index];
        }

        if (_currentTutorial.index < _currentTutorial.texts.Length)
        {
            _tutorialText.text = _currentTutorial.texts[_currentTutorial.index];
        }
        else
        {
            TutorialIndex++;
            CardGameState.ChangeGamePhase?.Invoke(GamePhase.Play);
        }
    }
}

[Serializable]
public struct TutorialScreen
{
    public VideoClip[] clips;
    [TextArea]
    public string[] texts;
    [HideInInspector]
    public int index;
}