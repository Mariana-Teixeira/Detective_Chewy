using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class TableScript : InteractableObject
{
    public Transform CardBodyPosition;
    [HideInInspector] public Transform CardCameraPosition;
    [HideInInspector] public Transform LookAtTarget;

    public void Start()
    {
        base.SetCamera();
        base.SetOutline();
        LookAtTarget = this.transform.GetChild(0);
        CardCameraPosition = this.transform.GetChild(2);
    }
}