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

    private void Start()
    {
        _tutorialCanvas = GetComponent<Canvas>();
        _videoPlayer = GetComponentInChildren<VideoPlayer>();

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
        }
        else
        {
            _tutorialCanvas.enabled = false;
            _videoPlayer.Stop();
        }
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
