using System;
using UnityEngine;

public enum GameState { NULL, WALKING, SITTING, PLAYING, INSPECTING, TALKING };
public class PlayerStates : MonoBehaviour
{
    public static Action PreviousState;
    public static Action<GameState> ChangeState;

    [SerializeField] DialogueManager _dialogueManager;

    CameraLook _cameraLook;
    PlayerMove _playerMove;
    InteractWith _interactWith;
    GameState _previousState;
    GameState _currentState;

    private void Awake()
    {
        PreviousState += OnPreviousChange;
        ChangeState += OnChangeState;
    }

    private void Start()
    {
        _cameraLook = GetComponent<CameraLook>();
        _playerMove = GetComponent<PlayerMove>();
        _interactWith = GetComponent<InteractWith>();

        OnChangeState(GameState.WALKING);
    }

    public GameState GetGameState() 
    {
        return _currentState;
    }

    // It's a temporary function, to avoid changing the current ChangeState Action.
    public void OnPreviousChange()
    {
        // I don't like this variable, but I also don't like this function, so...
        var ps = _previousState;
        ExitState();
        _currentState = ps;
        EnterState();
    }

    public void OnChangeState(GameState newState)
    {
        ExitState();
        _currentState = newState;
        EnterState();
    }

    private void Update()
    {
        UpdateState();
    }

    private void EnterState()
    {
        switch(_currentState)
        {
            case GameState.WALKING:
                _cameraLook.ToggleCursor(false);
                CardGameCanvasScript.ToggleVisibility?.Invoke(false);
                InspectorCanvasScript.ToggleVisibility?.Invoke(false);
                InformationCanvasScript.ToggleVisibility?.Invoke(true);
                break;
            case GameState.TALKING:
                InformationCanvasScript.ToggleVisibility?.Invoke(false);
                break;
            case GameState.INSPECTING:
                StartCoroutine(_cameraLook.ToggleInspecting());
                _cameraLook.ToggleCursor(true);
                break;
            case GameState.SITTING:
                StartCoroutine(_cameraLook.ToggleSitting());
                InformationCanvasScript.ToggleVisibility?.Invoke(false);
                break;
            case GameState.PLAYING:
                CardGameCanvasScript.ToggleVisibility?.Invoke(true);
                _cameraLook.ToggleCursor(true);
                break;
            default:
                Debug.LogError("Player State not found.");
                break;
        }
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case GameState.WALKING:
                _cameraLook.RotateWithMouse();
                _interactWith.CastInteractionRays();
                _playerMove.Move();
                break;
            case GameState.PLAYING:
                _interactWith.ListenForExitGame();
                break;
            case GameState.INSPECTING:
                _interactWith.ListenForExitInspect();
                break;
            case GameState.TALKING:
                _dialogueManager.ListenForNextDialogue();
                break;
            default:
                break;
        }
    }

    // Pai Nosso que estais no céu... T-T
    private void ExitState()
    {
        switch (_currentState)
        {
            case GameState.WALKING:
                _previousState = GameState.WALKING;
                break;
            case GameState.SITTING:
                _previousState = GameState.SITTING;
                break;  
            case GameState.INSPECTING:
                _previousState = GameState.INSPECTING;
                break;
            case GameState.PLAYING:
                _previousState = GameState.PLAYING;
                break;
            case GameState.TALKING:
                _previousState = GameState.TALKING;
                break;
            default:
                break;
        }
    }
}
