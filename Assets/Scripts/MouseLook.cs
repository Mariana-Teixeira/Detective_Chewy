using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{ 
    [SerializeField] float mouseSensitivity = 400;

    public Transform body;


    private float _xRotation = 0f;
    private bool _playingCards = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!_playingCards) { 
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        body.Rotate(Vector3.up * mouseX);
         }
    }

    public void UpdateLookAt(Transform pos) 
    {
        _playingCards = true;
        transform.parent.transform.LookAt(pos);
        transform.LookAt(pos);
        _xRotation = 0f;
        Vector3 eulerRotation = transform.parent.transform.rotation.eulerAngles;
        transform.parent.transform.rotation = Quaternion.Euler(0, eulerRotation.y,0 );
        transform.LookAt(pos);
        EnableCursor();
    }

    public void CancelCardGame() 
    {
        _playingCards = false;
        DisableCursor();
    }

    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsPlayingCards() 
    { 
        return _playingCards;
    }
}
