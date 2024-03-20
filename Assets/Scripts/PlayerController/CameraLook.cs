using System.Collections;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    Camera _camera;

    [SerializeField] float _mouseSensitivity = 400.0f;
    [SerializeField] float sittingDuration = 5.0f;
    public Transform InitialCardGamePosition { get; set; }
    public Transform LookAtTarget { get; set; }

    void Start()
    {
        _camera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    public void RotateWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * _mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        _camera.transform.Rotate(Vector3.left * mouseY);
    }

    public IEnumerator SittingAnimation()
    {
        var timeElapsed = 0.0f;
        var lookRotation = Quaternion.LookRotation(LookAtTarget.position - InitialCardGamePosition.position);

        while (timeElapsed < sittingDuration)
        {
            var t = timeElapsed / sittingDuration;
            transform.position = Vector3.Lerp(transform.position, InitialCardGamePosition.transform.position, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        PlayerStates.ChangeState?.Invoke(GameState.CARD);
    }
}
