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
        if (cursor.gameObject.activeInHierarchy) { cursor.gameObject.SetActive(false); }
        else { cursor.gameObject.SetActive(true); }
    }
}
