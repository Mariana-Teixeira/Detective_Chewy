using UnityEngine;

public class InteractWith : MonoBehaviour
{
    Camera _playerCamera;
    CameraLook _cameraLook;
    Ray _lookingAtRay;

    [SerializeField] float _rayDistance = 5.0f;
    [SerializeField] LayerMask _rayMask;

    private void Start()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _cameraLook = GetComponentInChildren<CameraLook>();
    }

    public void CastInteractionRays()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var centerViewport = new Vector3(Screen.width/2, Screen.height/2, 0);
            _lookingAtRay = _playerCamera.ScreenPointToRay(centerViewport);

            if (Physics.Raycast(_lookingAtRay, out RaycastHit hit, _rayDistance, _rayMask))
            {
                if (hit.collider.CompareTag("CardGame"))
                {
                    InteractWithChair(hit);
                }

            }
        }
    }

    void InteractWithChair(RaycastHit hit)
    {
        var table = hit.collider.GetComponent<TableScript>();
        _cameraLook.LookAtTarget = table.OpponentLookAtTarget;
        _cameraLook.InitialCardGamePosition = table.InitialCardGameCameraPosition;
        PlayerStates.ChangeState?.Invoke(GameState.SITTING);
    }
}
