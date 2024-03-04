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
    [SerializeField] Transform InteractorSource;
    [SerializeField] float InteractRange;
    void Start()
    {
        
    }

    void Update()
    {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable obj))
            {
                obj.ShowInfo();
            }

        }


            if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (Physics.Raycast(r, out RaycastHit hitInfo1, InteractRange)) 
            {
                //Debug.Log(hitInfo.collider.gameObject.name);
                if (hitInfo1.collider.gameObject.TryGetComponent(out IInteractable obj)) 
                {
                    Debug.Log(transform.parent.name);
                    Debug.Log(transform.parent.transform.localPosition);
                    Debug.Log(hitInfo1.collider.gameObject.transform.localPosition);
                    /*
                    transform.parent.transform.localPosition = new Vector3(hitInfo1.collider.gameObject.transform.position.x, hitInfo1.collider.gameObject.transform.position.y, hitInfo1.collider.gameObject.transform.position.z);
                    */

                    obj.Interact();
                    obj.HideInfo();

                }
            }
        }
        
    }
}
