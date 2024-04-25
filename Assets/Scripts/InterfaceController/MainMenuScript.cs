using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject _menuPanel;
    //[SerializeField] CameraTransition _cameraTransition;
    [SerializeField] Animator _animator;

    public float transitionTime = 1.5f;

    private int state1 = 0;
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
        _menuPanel.SetActive(true);
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            state1 = 1;
        }
        else { state1 = 0; }
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void Continue()
    {
        _menuPanel.SetActive(false);
        if (state1 == 1) { }
        else {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        }
        Time.timeScale = 1;
        //_cameraTransition.DisableCursor();
    }

    public void BackToMainMenu() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    IEnumerator LoadLevel(int LevelIndex) 
    {
        _animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(LevelIndex);
    }
}
