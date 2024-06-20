using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSettings : MonoBehaviour
{
    [SerializeField] Button backBtn;

    [SerializeField] GameObject menuPanel;
    void Start()
    {
        backBtn.onClick.AddListener(BackToMenu);
    }

    void BackToMenu()
    {
        menuPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
