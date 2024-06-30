using System.Collections;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    Camera _camera;

    [SerializeField] float xRotationSensitivity = 45.0f, yRotationSensitivity = 100.0f;
    [SerializeField] float xUpperClamp = 10f, xLowerClamp = -10f;
    float rotationX, rotationY;

    [SerializeField] float fovWalking = 60, fovPlaying = 35;
    [SerializeField] float sittingDuration = 2.0f, zoomingDuration = 1.0f;
    [SerializeField] InteractWith interactWith;

    private Vector3 GameBodyPosition, GameCameraPosition;
    public Transform CardBodyTransform, CardCameraTransform;
    public Transform LookAtTarget;

    private bool _isSitting;
    private bool _seenTutorial;

    private Quaternion _beforeSittingCameraAngle;

    void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _camera.fieldOfView = fovWalking;

        _camera.transform.rotation = Quaternion.Euler(Vector3.zero);
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
        rotationX -= Input.GetAxis("Mouse Y") * xRotationSensitivity;
        rotationX = Mathf.Clamp(rotationX, xLowerClamp, xUpperClamp);
        _camera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        
        rotationY = Input.GetAxis("Mouse X") * yRotationSensitivity;
        transform.rotation *= Quaternion.Euler(0f, rotationY, 0f);
    }
    public IEnumerator ToggleSitting()
    {
        if (_isSitting) // Get Up
        {
            // Set Variables
            var lookAtRotation = _beforeSittingCameraAngle;
            

            // Call Coroutines
            yield return StartCoroutine(ZoomAnimation(fovPlaying, fovWalking));
            yield return StartCoroutine(SittingAnimation(lookAtRotation, GameBodyPosition, GameCameraPosition));

            // Change State
            _isSitting = !_isSitting;
            PlayerStates.ChangeState?.Invoke(GameState.WALKING);
        }
        else // Sit Down
        {
            // Set Variables
            GameBodyPosition = transform.position;
            GameCameraPosition = _camera.transform.position;
            _beforeSittingCameraAngle = this.transform.rotation;
            var lookAtRotation = Quaternion.LookRotation(LookAtTarget.position - CardCameraTransform.position);

            // Call Coroutines
            yield return StartCoroutine(SittingAnimation(lookAtRotation, CardBodyTransform.position, CardCameraTransform.position));
            yield return StartCoroutine(ZoomAnimation(fovWalking, fovPlaying));

            // Change State
            _isSitting = !_isSitting;
            if (_seenTutorial) PlayerStates.ChangeState?.Invoke(GameState.PLAYING);
            else { PlayerStates.ChangeState?.Invoke(GameState.TUTORIAL); _seenTutorial = !_seenTutorial; }
        }
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
            var t = Mathf.Lerp(0, 1, BadMath.LerpOutSmooth(timeElapsed, sittingDuration));
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
            var t = BadMath.LerpInSmooth(timeElapsed, zoomingDuration);
            _camera.fieldOfView = Mathf.Lerp(fovStart, fovEnd, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        _camera.fieldOfView = 35f;
    }
}
