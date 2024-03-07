using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

interface IInteractable {
    public void Interact(); //Activity after clicking E
    public void ShowInfo(); //Show the hover effect
    public bool GetInfo();  //Boolean of info State, TRUE -> ON, FALSE -> OFF
    public void HideInfo(); //Hide the hover effect
}
public class Interactor : MonoBehaviour
{
    [Header("Camera/Interactor Settings")]
    [SerializeField] Transform interactorSource;
    [SerializeField] float interactRange;

    [Header("Non Playable Characters")]
    [SerializeField] GameObject[] NPCs;

    [Header("Managers")]
    [SerializeField] MenuManager menuManager;

    [Header("Vars")]
    [SerializeField] float durationOfLerp;

    MouseLook ml = null;
    void Start()
    {
        ml = GetComponent<MouseLook>();
    }

    void Update()
    {
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable obj))
            {
                if (obj.GetInfo()) { }
                else 
                {
                    obj.ShowInfo();
                }
            }

        }


        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (Physics.Raycast(r, out RaycastHit hitInfo1, interactRange)) 
            {
                //Debug.Log(hitInfo.collider.gameObject.name);
                if (hitInfo1.collider.gameObject.TryGetComponent(out IInteractable obj)) 
                {
                    obj.Interact();
                    obj.HideInfo();
                    StartCoroutine(LerpPosition(hitInfo1));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !ml.IsPlayingCards()) 
        {
            menuManager.OpenPauseMEnu();
            ml.EnableCursor();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && ml.IsPlayingCards()) 
        {
            ml.CancelCardGame();
            transform.position = transform.position + new Vector3(0, 0.5f, 0);
        }

        
    }

    IEnumerator LerpPosition(RaycastHit hitInfo1) 
    {
        int npcNum = Int32.Parse(hitInfo1.collider.gameObject.name.Substring(hitInfo1.collider.gameObject.name.Length - 1)) - 1;
        GameObject chair = hitInfo1.collider.gameObject;
        ml.UpdateLookAt(NPCs[npcNum].transform, durationOfLerp, chair);
        yield return null;
    }
}
