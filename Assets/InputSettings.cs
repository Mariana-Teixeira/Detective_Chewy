using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputSettings : MonoBehaviour
{
    [SerializeField] Button backBtn;

    [SerializeField] GameObject menuPanel;
    [SerializeField] MenuManager menuManager;

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button RebindButton;
    [SerializeField] GameObject WaitForInputButton;

    void Start()
    {
        backBtn.onClick.AddListener(BackToMenu);
        RebindButton.onClick.AddListener(WaitForInput);
    }

    void BackToMenu()
    {
        menuManager.StoreValues();
        menuPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void WaitForInput()
    {
        RebindButton.gameObject.SetActive(false);
        WaitForInputButton.SetActive(true);
    }
}
