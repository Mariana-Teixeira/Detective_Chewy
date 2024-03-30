using UnityEngine;

public class TableScript : InteractableObject
{
    public Transform CardBodyPosition;
    [HideInInspector] public Transform CardCameraPosition;
    [HideInInspector] public Transform LookAtTarget;

    private void Start()
    {
        LookAtTarget = this.transform.GetChild(0);
        CardCameraPosition = this.transform.GetChild(2);
    }
}