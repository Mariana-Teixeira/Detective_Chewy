using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable {
    public void Interact();
    public void ShowInfo();
    public void HideInfo();
}
public class Interactor : MonoBehaviour
{
    [SerializeField] Transform interactorSource;
    [SerializeField] float interactRange;
    [SerializeField] GameObject npc1;

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
                obj.ShowInfo();
            }

        }


            if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (Physics.Raycast(r, out RaycastHit hitInfo1, interactRange)) 
            {
                //Debug.Log(hitInfo.collider.gameObject.name);
                if (hitInfo1.collider.gameObject.TryGetComponent(out IInteractable obj)) 
                {
                    
                    transform.parent.transform.position = hitInfo1.collider.gameObject.transform.position;
                    obj.Interact();
                    obj.HideInfo();
                    ml.UpdateLookAt(npc1.transform);



                }
            }
        }
        
    }
}
