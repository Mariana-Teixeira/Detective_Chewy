using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] MouseLook mouseLook;
    [SerializeField] Animator animator;

    public float transitionTime = 1.5f;
    public void PlayGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 2));
    }
    public void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }

    public void Pause()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
        mouseLook.DisableCursor();
    }

    public void BackToMainMenu() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    IEnumerator LoadLevel(int LevelIndex) 
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(LevelIndex);
    }
}
