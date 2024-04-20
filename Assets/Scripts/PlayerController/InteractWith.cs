using UnityEngine;

public class InteractWith : MonoBehaviour
{
    Camera _playerCamera;
    CameraLook _cameraLook;
    Ray _lookingAtRay;


    [SerializeField] Deck gameDeck;

    [SerializeField] float _rayDistance;
    [SerializeField] LayerMask _rayMask;

    private void Start()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _cameraLook = GetComponentInChildren<CameraLook>();
    }

    public void CastInteractionRays()
    {
        //Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction * _rayDistance);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("------------------------");
            var centerViewport = new Vector3(Screen.width/2, Screen.height/2, 0);
            _lookingAtRay = _playerCamera.ScreenPointToRay(centerViewport);

            if (Physics.Raycast(_lookingAtRay, out RaycastHit hit, _rayDistance, _rayMask))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.CompareTag("CardGame"))
                {
                    InteractWithTable(hit);
                }
            }
        }
    }

    void InteractWithTable(RaycastHit hit)
    {
        TableScript table = hit.collider.GetComponent<TableScript>();
        _cameraLook.LookAtTarget = table.LookAtTarget;
        _cameraLook.CardCameraTransform = table.CardCameraPosition;
        _cameraLook.CardBodyTransform = table.CardBodyPosition;
        gameDeck.RandomOnNewBoard(table);
        PlayerStates.ChangeState?.Invoke(GameState.SITTING);
    }

    public void ListenForExitGame()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerStates.ChangeState?.Invoke(GameState.SITTING);
        }
    }
}
