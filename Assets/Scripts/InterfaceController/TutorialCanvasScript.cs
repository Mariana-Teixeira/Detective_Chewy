using System;
using UnityEngine;
using UnityEngine.Video;

public class TutorialCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;
    public static Action ClickToNext;
    private Canvas _tutorialCanvas;
    private VideoPlayer _videoPlayer;
    public VideoClip[] TutorialVideos;
    private int VideoIndex = 0;

    private Animator _animator;

    private void Start()
    {
        _tutorialCanvas = GetComponent<Canvas>();
        _videoPlayer = GetComponentInChildren<VideoPlayer>();
        _animator = GetComponentInChildren<Animator>();

        ToggleVisibility += OnToggleVisibility;
        ClickToNext += OnClickToNext;

        UpdateVideo();
    }

    public void OnToggleVisibility(bool isVisible)
    {
        if (isVisible)
        {
            _tutorialCanvas.enabled = true;
            _videoPlayer.Play();
            _animator.SetTrigger("fadein");
        }
        else
        {
            _videoPlayer.Stop();
            _animator.SetTrigger("fadeout");
        }
    }

    public void ActivateCanvas()
    {
        _tutorialCanvas.enabled = true;
    }

    public void DeactivateCanvas()
    {
        _tutorialCanvas.enabled = false;
    }

    public void OnClickToNext()
    {
        VideoIndex++;

        if(VideoIndex < TutorialVideos.Length)
        {
            UpdateVideo();
        }
        else
        {
            PlayerStates.ChangeState?.Invoke(GameState.PLAYING);
        }
    }

    public void UpdateVideo()
    {
        _videoPlayer.clip = TutorialVideos[VideoIndex];
    }
}
