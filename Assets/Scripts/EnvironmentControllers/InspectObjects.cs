using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectObjects : MonoBehaviour
{

    protected Vector3 posLastFrame;
    public Camera UICam;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            posLastFrame = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) {
            var delta = Input.mousePosition - posLastFrame;
            posLastFrame = Input.mousePosition;

            var axis = Quaternion.AngleAxis(-90f, Vector3.forward) * delta;

            transform.rotation = Quaternion.AngleAxis(delta.magnitude * 0.1f, axis) * transform.rotation;
            //Debug.Log(transform.rotation);
        }
    }
}
