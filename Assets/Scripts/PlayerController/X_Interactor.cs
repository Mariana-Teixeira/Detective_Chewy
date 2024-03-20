using System;
using System.Collections;
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

    CameraTransition _cameraTransition = null;
    void Start()
    {
        _cameraTransition = GetComponent<CameraTransition>();
    }

    void Update()
    {
        //Hover effects
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

        //Interact with E if object had Interactable component
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

        //Escape to exit card game or open main menu outside card game 
        //Might have to switch it to always open main menu, add X to cardUI for exiting the card game, as main menu is not accesibile from card game this way
        if (Input.GetKeyDown(KeyCode.Escape) && !_cameraTransition.IsPlayingCards()) 
        {
            menuManager.OpenPauseMEnu();
            _cameraTransition.EnableCursor();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && _cameraTransition.IsPlayingCards()) 
        {
            _cameraTransition.CancelCardGame(durationOfLerp);
            //transform.position = transform.position + new Vector3(0, 0.5f, 0);
        }

        //Change view inside card game to Bird CAM
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            if (_cameraTransition.IsPlayingCards() && _cameraTransition.IsBasicView()) 
            { 
                _cameraTransition.SwitchToBirdCamera();
            }
        }

        //Change view inside card game to Basic CAM
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(_cameraTransition.IsPlayingCards() && _cameraTransition.IsBirdView()) 
            { 
            _cameraTransition.SwitchToBasicCamera();
            }
        }


    }

    IEnumerator LerpPosition(RaycastHit hitInfo1) 
    {
        int npcNum = Int32.Parse(hitInfo1.collider.gameObject.name.Substring(hitInfo1.collider.gameObject.name.Length - 1)) - 1;
        GameObject chair = hitInfo1.collider.gameObject;
        _cameraTransition.UpdateLookAt(NPCs[npcNum].transform, durationOfLerp, chair);
        yield return null;
    }
}
