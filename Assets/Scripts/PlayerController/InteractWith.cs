using UnityEngine;

public class InteractWith : MonoBehaviour
{
    Camera _playerCamera;
    CameraLook _cameraLook;
    Ray _lookingAtRay;

    [SerializeField] float _rayDistance;
    [SerializeField] LayerMask _rayMask;

    private GameObject _currentInspectingObject;

    private void Start()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _cameraLook = GetComponentInChildren<CameraLook>();
    }

    public void CastInteractionRays()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var centerViewport = new Vector3(Screen.width/2, Screen.height/2, 0);
            _lookingAtRay = _playerCamera.ScreenPointToRay(centerViewport);

            if (Physics.Raycast(_lookingAtRay, out RaycastHit hit, _rayDistance, _rayMask))
            {
                if (hit.collider.CompareTag("CardGame"))
                {
                    InteractWithTable(hit);
                }
                if (hit.collider.CompareTag("Clue"))
                {
                    InteractWithClue(hit);
                }
                if (hit.collider.CompareTag("Character"))
                {
                    InteractWithCharacter(hit);
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
        
        if (table.PlayGame())
        {
            PlayerStates.ChangeState?.Invoke(GameState.SITTING);
        }

    }
    void InteractWithClue(RaycastHit hit)
    {
        ClueScript item = hit.collider.gameObject.GetComponent<ClueScript>();
        item.GatherClue();
    }

    void InteractWithCharacter(RaycastHit hit)
    {
        CharacterScript character = hit.collider.gameObject.GetComponent<CharacterScript>();
        character.TalkToCharacter();
    }

    public void ListenForExitGame()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerStates.ChangeState?.Invoke(GameState.SITTING);
        }
    }
}
