using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionScript : MonoBehaviour
{
    public void OnStartLoading(int i)
    {
        StartCoroutine(LoadLevelAsync(i));
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