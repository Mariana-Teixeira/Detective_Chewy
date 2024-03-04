using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChairScript : MonoBehaviour, IInteractable 
{
    [SerializeField] GameObject infoField;

    private bool _infoOn = false;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void Interact()
    {
        _infoOn = false;
        SitPlayerDown();
    }

    public void SitPlayerDown() 
    {
        Debug.Log("Player sits down");
        // Change position of player to sit down
        // Change UI to add cards
        // Add option to leave card game
    }

    public void ShowInfo()
    {
        if (!_infoOn) { 
            infoField.SetActive(true);
            Invoke("HideInfo", 3);
            _infoOn = true;
        }
    }
    public void HideInfo()
    {
        _infoOn = false;
        infoField.SetActive(false);
    }

}
