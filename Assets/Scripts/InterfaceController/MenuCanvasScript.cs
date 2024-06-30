using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuCanvasScript : MonoBehaviour
{
    VideoPlayer _videoPlayer;
    Animator _animator;
    RawImage _cutscene;

    private void Awake()
    {
        _videoPlayer = GetComponentInChildren<VideoPlayer>();
        _animator = GetComponent<Animator>();
        _cutscene = this.transform.GetChild(3).GetComponent<RawImage>();
    }

    public void StartLoading()
    {
        StartCoroutine(LoadLevelAsync(1));
    }

    public void StartGame()
    {
        StartCoroutine(StartAnimation());
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
            Debug.Log(progressValue);
            yield return null;
        }
    }
}
