using Unity.VisualScripting;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    public Transform InitialCardGameCameraPosition;
    public Transform OpponentLookAtTarget;

    private void Start()
    {
        InitialCardGameCameraPosition = GameObject.Find("InitialCameraPosition").transform;
        OpponentLookAtTarget = GameObject.Find("TargetLookAt").transform;
    }
}