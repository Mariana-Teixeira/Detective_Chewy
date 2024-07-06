using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputSettings : MonoBehaviour
{
    [SerializeField] GameObject cursor;
    [SerializeField] Toggle toggle;
    void Start()
    {
        //RebindButton.onClick.AddListener(WaitForInput);
        //toggle.isOn = StaticData.InteractHelp;
    }

    public void EnhanceCursor()
    {
        if (toggle.isOn) { 
            cursor.gameObject.SetActive(false);
            StaticData.InteractHelp = false;
            toggle.isOn = false;
        }
        else { 
            cursor.gameObject.SetActive(true);
            StaticData.InteractHelp = true;
            toggle.isOn = true;
        }
    }

    public bool GetToggle()
    {
        return toggle;
    }
}
