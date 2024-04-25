using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] settingsScript menuSettings;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }
    public void StoreValues() 
    {
        StaticData.Volume = menuSettings.GetVolume();
        StaticData.Subtitles = menuSettings.GetSubtitles();
    }

    public void OpenPauseMenu() 
    {
        menuPanel.GetComponent<MainMenuScript>().Pause();
    }

}
