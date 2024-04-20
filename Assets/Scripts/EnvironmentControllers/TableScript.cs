using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TableScript : InteractableObject
{
    public Transform CardBodyPosition;
    [HideInInspector] public Transform CardCameraPosition;
    [HideInInspector] public Transform LookAtTarget;

    private void Start()
    {
        LookAtTarget = this.transform.GetChild(0);
        CardCameraPosition = this.transform.GetChild(2);
        base.setOutline();
    }


}