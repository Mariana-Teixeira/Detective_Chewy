using UnityEngine;
using UnityEngine.InputSystem;

public class InteractWith : MonoBehaviour
{
    Camera _playerCamera;
    CameraLook _cameraLook;
    Ray _lookingAtRay;


    [SerializeField] float _rayDistance;
    [SerializeField] LayerMask _rayMask;

    private string _rebindedInput;

    private void Start()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _cameraLook = GetComponentInChildren<CameraLook>();

        _rebindedInput = StaticData.InteractBtn;
    }

    public void CastInteractionRays()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire1"))
        {
            var centerViewport = new Vector3(Screen.width/2, Screen.height/2, 0);
            _lookingAtRay = _playerCamera.ScreenPointToRay(centerViewport);

            if (Physics.Raycast(_lookingAtRay, out RaycastHit hit, _rayDistance, _rayMask))
            {
                if (hit.collider.CompareTag("CardGame"))
                {
                    InteractWithTable(hit);
                }
                else if (hit.collider.CompareTag("Clue"))
                {
                    InteractWithClue(hit);
                }
                else if (hit.collider.CompareTag("Character"))
                {
                    InteractWithCharacter(hit);
                }
            }
        }
    }

    // Bad bad bad
    public void CastCursorRays()
    {
        var centerViewport = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        _lookingAtRay = _playerCamera.ScreenPointToRay(centerViewport);

        if (Physics.Raycast(_lookingAtRay, out RaycastHit hit, _rayDistance, _rayMask))
        {
            CursorCanvas.ChangeCursor?.Invoke(true);
        }
        else
        {
            CursorCanvas.ChangeCursor?.Invoke(false);
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

    public void ListenForTutorial()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TutorialCanvasScript.ClickToNext?.Invoke();
        }
    }
}
