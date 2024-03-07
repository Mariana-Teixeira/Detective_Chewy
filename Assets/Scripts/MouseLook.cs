using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MouseLook : MonoBehaviour
{ 
    [SerializeField] float mouseSensitivity = 400;
    [SerializeField] float speedMoveSit;
    [SerializeField] float speedRotateSit;

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

    public void UpdateLookAt(Transform pos, float durationOfLerp, GameObject sitPos)
    {
        _playingCards = true;
        StartCoroutine(LookAtLerp(pos, durationOfLerp, sitPos));
    }

    private IEnumerator LookAtLerp(Transform pos, float durationOfLerp, GameObject sitPos) 
    {
        /*
        transform.parent.transform.LookAt(pos);
        Vector3 eulerRotation = transform.parent.transform.rotation.eulerAngles;
        transform.parent.transform.rotation = Quaternion.Euler(0, eulerRotation.y,0 );
        transform.LookAt(pos);
        */
        Quaternion lookRotationParent = Quaternion.LookRotation(pos.position - transform.parent.transform.position);
        lookRotationParent.x = 0;
        lookRotationParent.z = 0;
        Debug.Log(pos.position);

        float time = 0;

        Vector3 startPosition = transform.parent.transform.position;
        Vector3 endPosition = sitPos.transform.position + new Vector3(0, 1.5f, 0);

        while (time < durationOfLerp)
        {
            transform.parent.transform.position = Vector3.Lerp(startPosition, endPosition, time / durationOfLerp);
            transform.parent.transform.rotation = Quaternion.Slerp(transform.parent.transform.rotation, lookRotationParent, time/50);
            time += Time.deltaTime * speedMoveSit;

            lookRotationParent = Quaternion.LookRotation(pos.position - transform.parent.transform.position);
            lookRotationParent.x = 0;
            lookRotationParent.z = 0;
            yield return null;
        }

        //fix it on a correct position after Lerp
        transform.parent.transform.LookAt(pos);
        Vector3 eulerRotation = transform.parent.transform.rotation.eulerAngles;
        transform.parent.transform.rotation = Quaternion.Euler(0, eulerRotation.y, 0);
        //

        time = 0;
        Quaternion lookRotation = Quaternion.LookRotation(pos.position - transform.position);
        Vector3 newStartPosition = transform.position;
        Vector3 newEndPosition = transform.position - new Vector3(0,0.5f,0);
        while (time < durationOfLerp)
        {
            transform.position = Vector3.Lerp(newStartPosition, newEndPosition, time / durationOfLerp);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * speedRotateSit;

            lookRotation = Quaternion.LookRotation(pos.position - transform.position);
            yield return null;
        }
       
        //transform.parent.transform.position = sitPos.transform.position + new Vector3(0, 1.5f, 0);
        _xRotation = 0f;
        EnableCursor();
        yield return null;
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
