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
    }

    public void EnhanceCursor()
    {
        if (StaticData.InteractHelp == true) { 
            cursor.gameObject.SetActive(false);
            StaticData.InteractHelp = false;
        }
        else { 
            cursor.gameObject.SetActive(true);
            StaticData.InteractHelp = true;
        }
    }
}
