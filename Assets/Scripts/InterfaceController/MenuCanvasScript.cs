using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuCanvasScript : MonoBehaviour
{
    public MenuManager MenuManager;

    public Canvas MenuCanvas;
    public CanvasGroup _mainCanvas;
    public CanvasGroup _audioCanvas;
    public CanvasGroup _inputCanvas;

    public VideoPlayer _videoPlayer;
    public Animator _animator;
    public RawImage _cutscene;

    public void EnableCanvas()
    {
        MenuCanvas.enabled = true;
        PlayerStates.ChangeState?.Invoke(GameState.MENU);
    }

    public void DisableCanvas(GameState state)
    {
        MenuCanvas.enabled = false;
        ChangeCanvas(0);
        PlayerStates.ChangeState?.Invoke(state);
    }

    public void ChangeCanvas(int i)
    {
        switch (i)
        {
            case (int)MenuStates.MainMenu:
                EnableCanvas(_mainCanvas);
                DisableCanvas(_audioCanvas);
                DisableCanvas(_inputCanvas);

                break;
            case (int)MenuStates.AudioSettings:
                DisableCanvas(_mainCanvas);
                EnableCanvas(_audioCanvas);
                DisableCanvas(_inputCanvas);
                break;
            case (int)MenuStates.InputSettings:
                DisableCanvas(_mainCanvas);
                DisableCanvas(_audioCanvas);
                EnableCanvas(_inputCanvas);
                break;
        }
    }

    public void EnableCanvas(CanvasGroup group)
    {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    public void DisableCanvas(CanvasGroup group)
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    public void StartLoading()
    {
        StartCoroutine(LoadLevelAsync(1));
    }

    public void StartGame()
    {
        StartCoroutine(StartAnimation());
    }

    public void ResumeGame()
    {
        MenuManager.ToggleCanvas();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public IEnumerator StartAnimation()
    {
        yield return StartCoroutine(VideoPlaying());
        _animator.SetTrigger("card_start");
    }

    IEnumerator VideoPlaying()
    {
        _videoPlayer.Prepare();

        while (!_videoPlayer.isPrepared)
        {
            yield return null;
        }

        _videoPlayer.Play();
        _cutscene.enabled = true;

        while (_videoPlayer.isPlaying)
        {
            yield return null;
        }
    }

    IEnumerator LoadLevelAsync(int loadScene)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(loadScene);
        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            yield return null;
        }
    }
}
