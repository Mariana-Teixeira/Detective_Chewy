using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public enum TransitionState { ToGame, FromGame }

public class MenuCanvasScript : MonoBehaviour
{
    public static Action<TransitionState> PlayTransition;

    public MenuManager MenuManager;

    public Canvas MenuCanvas;
    public CanvasGroup _mainCanvas;
    public CanvasGroup _audioCanvas;
    public CanvasGroup _inputCanvas;

    public VideoPlayer _videoPlayer;
    public Animator _animator;
    public RawImage _cutscene;

    private void Awake()
    {
        ChangeCanvas(0);

        // Temporary code for the GameDev Meet Porto
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        PlayTransition += OnPlayTransition;
    }

    private void OnDisable()
    {
        PlayTransition -= OnPlayTransition;
    }

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

    public void StartGame()
    {
        OnPlayTransition(TransitionState.ToGame);

        // Temporary code for the GameDev Meet Porto
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnPlayTransition(TransitionState state)
    {
        if (state == TransitionState.ToGame)
        {
            StartCoroutine(StartAnimation());
        }
        else
        {
            _animator.SetTrigger("transition");
        }
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
        _animator.SetTrigger("transition");
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
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)) yield break;

            yield return null;
        }
    }
}
