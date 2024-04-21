using UnityEngine;

public class InteractWith : MonoBehaviour
{
    Camera _playerCamera;
    CameraLook _cameraLook;
    Ray _lookingAtRay;


    [SerializeField] Deck gameDeck;

    [SerializeField] float _rayDistance;
    [SerializeField] LayerMask _rayMask;

    private GameObject _lastGameobject;

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
            var centerViewport = new Vector3(Screen.width/2, Screen.height/2, 0);
            _lookingAtRay = _playerCamera.ScreenPointToRay(centerViewport);

            if (Physics.Raycast(_lookingAtRay, out RaycastHit hit, _rayDistance, _rayMask))
            {
                if (hit.collider.CompareTag("CardGame"))
                {
                    InteractWithTable(hit);
                }
                if (hit.collider.CompareTag("InspectableItem"))
                {
                    InteractWithObject(hit);
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
    void InteractWithObject(RaycastHit hit)
    {
        GameObject item = hit.collider.gameObject;
        _lastGameobject = item;
        PlayerStates.ChangeState?.Invoke(GameState.INSPECTING);
    }


    public void ListenForExitGame()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerStates.ChangeState?.Invoke(GameState.SITTING);
        }
    }

    public void ListenForExitInspect() 
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerStates.ChangeState?.Invoke(GameState.WALKING);
        }
    }
    public GameObject getLastInteracted() {
        return _lastGameobject;
    }
}
