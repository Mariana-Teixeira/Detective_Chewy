using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 6;
    [SerializeField] bool canCrouch = true;
    [SerializeField] MouseLook mouseLook;

    private float _startY;
    // In case we want gravity
    //[SerializeField] float gravity = -9.81f;
    //private Vector3 _velocity; 

    void Update()
    {
        if (!mouseLook.IsPlayingCards()) { 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right* x + transform.forward* z;

        controller.Move(move*speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl) && canCrouch) 
        {
            _startY = transform.localScale.y;
            //change model to crouched
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y/4, transform.localScale.z);
            speed = 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && canCrouch)
        {
            transform.localScale = new Vector3(transform.localScale.x, _startY, transform.localScale.z);
            speed = 6;
        }
        }
        // In case we want gravity
        //_velocity.y += gravity * Time.deltaTime;
        //controller.Move(_velocity * Time.deltaTime);

    }
}
