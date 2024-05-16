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
        if (gameDeck.CheckIfTableCanBePlayed(hit.collider.gameObject.name))
        {
            TableScript table = hit.collider.GetComponent<TableScript>();
            _cameraLook.LookAtTarget = table.LookAtTarget;
            _cameraLook.CardCameraTransform = table.CardCameraPosition;
            _cameraLook.CardBodyTransform = table.CardBodyPosition;
            PlayerStates.ChangeState?.Invoke(GameState.SITTING);
            gameDeck.RandomOnNewBoard(table);
        }
    }
    void InteractWithClue(RaycastHit hit)
    {
        GameObject item = hit.collider.gameObject;
        item.GetComponent<ClueScript>().GatherClue();
        _lastGameobject = item;
        PlayerStates.ChangeState?.Invoke(GameState.INSPECTING);

        Debug.Log("Found Clue: " + item.name);
    }

    void InteractWithCharacter(RaycastHit hit)
    {
        CharacterScript character = hit.collider.gameObject.GetComponent<CharacterScript>();
        character.TalkToCharacter();
        PlayerStates.ChangeState?.Invoke(GameState.TALKING);
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
    public GameObject GetLastInteracted()
    {
        return _lastGameobject;
    }
}
