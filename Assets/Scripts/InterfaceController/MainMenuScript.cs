using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject _menuPanel;
    //[SerializeField] CameraTransition _cameraTransition;
    [SerializeField] Animator _animator;

    [SerializeField] Button backToGameBtn;
    [SerializeField] Button audioSettingsBtn;
    [SerializeField] Button inputSettingsBtn;
    [SerializeField] Button exitGameBtn;


    [SerializeField] GameObject _audioPanel;
    [SerializeField] GameObject _inputPanel;

    public float transitionTime = 1.5f;

    private int state1 = 0;

    private void Start()
    {
        backToGameBtn.onClick.AddListener(Continue);
        audioSettingsBtn.onClick.AddListener(OpenAudioSettings);
        inputSettingsBtn.onClick.AddListener(OpenInputSettings);
        exitGameBtn.onClick.AddListener(BackToMainMenu);

        _menuPanel.SetActive(true);
        _audioPanel.SetActive(false);
        _inputPanel.SetActive(false);
    }

    public void PlayGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    void OpenAudioSettings() {
        _audioPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void OpenInputSettings() {
        _inputPanel.SetActive(true);
        this.gameObject.SetActive(false);
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
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        else { 
        if (state1 == 1) { }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        Time.timeScale = 1;
        _menuPanel.SetActive(false);
        //_cameraTransition.DisableCursor();
        }
    }

    public void BackToMainMenu() 
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Application.Quit();
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    IEnumerator LoadLevel(int LevelIndex) 
    {
        _animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(LevelIndex);
    }


}
