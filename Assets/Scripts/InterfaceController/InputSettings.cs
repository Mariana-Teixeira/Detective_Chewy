using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputSettings : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button RebindButton;

    [SerializeField] GameObject WaitForInputButton;

    void Start()
    {
        //RebindButton.onClick.AddListener(WaitForInput);
    }

    void WaitForInput()
    {
        RebindButton.gameObject.SetActive(false);
        WaitForInputButton.SetActive(true);
    }
}
