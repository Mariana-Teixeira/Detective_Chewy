using System.Collections;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    Camera _camera;

    [SerializeField] float _mouseSensitivity = 400.0f;
    [SerializeField] float fovWalking = 60, fovPlaying = 35;
    [SerializeField] float sittingDuration = 2.0f, zoomingDuration = 1.0f; // was 5, 2
    [SerializeField] InteractWith interactWith;

    private Vector3 GameBodyPosition, GameCameraPosition;
    public Transform CardBodyTransform, CardCameraTransform;
    public Transform LookAtTarget;

    private bool _isSitting;

    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _camera.fieldOfView = fovWalking;
    }

    public void ToggleCursor(bool visible)
    {
        if (visible)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void RotateWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * _mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        _camera.transform.Rotate(Vector3.left * mouseY);
    }
    public IEnumerator ToggleSitting()
    {
        if (_isSitting) // Sit Player
        {
            // Set Variables
            var lookAtRotation = Quaternion.LookRotation(LookAtTarget.position - GameCameraPosition);

            // Call Coroutines
            yield return StartCoroutine(ZoomAnimation(fovPlaying, fovWalking));
            yield return StartCoroutine(SittingAnimation(lookAtRotation, GameBodyPosition, GameCameraPosition));

            // Change State
            _isSitting = !_isSitting;
            PlayerStates.ChangeState?.Invoke(GameState.WALKING);
        }
        else // Get Up
        {
            // Set Variables
            GameBodyPosition = transform.position;
            GameCameraPosition = _camera.transform.position;
            var lookAtRotation = Quaternion.LookRotation(LookAtTarget.position - CardCameraTransform.position);

            // Call Coroutines
            yield return StartCoroutine(SittingAnimation(lookAtRotation, CardBodyTransform.position, CardCameraTransform.position));
            yield return StartCoroutine(ZoomAnimation(fovWalking, fovPlaying));

            // Change State
            _isSitting = !_isSitting;
            PlayerStates.ChangeState?.Invoke(GameState.PLAYING);
        }
    }
    public IEnumerator ToggleInspecting()
    {
        yield return StartCoroutine(EnableInspect());
    }

    private IEnumerator EnableInspect()
    {
        InspectorCanvasScript.InspectItem?.Invoke(interactWith.GetLastInteracted());
        yield return null;
    }

    private IEnumerator SittingAnimation(Quaternion lookAtRotation, Vector3 targetBodyPosition, Vector3 targetCameraPosition)
    {
        var initialBodyPosition = transform.position;
        var initialCameraPosition = _camera.transform.position;
        var initialCameraRotation = _camera.transform.rotation;

        var timeElapsed = 0.0f;
        var lookRotation = lookAtRotation;

        while (timeElapsed < sittingDuration)
        {
            var t = timeElapsed / sittingDuration;
            transform.position = Vector3.Lerp(initialBodyPosition, targetBodyPosition, t);
            _camera.transform.position = Vector3.Lerp(initialCameraPosition, targetCameraPosition, t);
            _camera.transform.rotation = Quaternion.Lerp(initialCameraRotation, lookRotation, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator ZoomAnimation(float fovStart, float fovEnd)
    {
        var timeElapsed = 0.0f;

        while (timeElapsed < zoomingDuration)
        {
            var t = timeElapsed / zoomingDuration;
            _camera.fieldOfView = Mathf.Lerp(fovStart, fovEnd, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
