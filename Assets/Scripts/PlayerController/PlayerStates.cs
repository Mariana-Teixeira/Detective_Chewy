using System;
using UnityEngine;

public enum GameState { NULL, WALKING, SITTING, TUTORIAL, PLAYING, DEBATING, TALKING };
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

    bool _currentlyPlaying;

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
        var previousState = _previousState;
        ExitState();
        _currentState = previousState;
        EnterState();
    }

    public void OnChangeState(GameState newState)
    {
        if (_currentState == newState) return;

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
                InformationCanvasScript.ToggleVisibility?.Invoke(true);
                DialogueCanvasScript.ToggleVisibility?.Invoke(false);
                break;
            case GameState.TALKING:
                DialogueCanvasScript.ToggleVisibility?.Invoke(true);
                break;
            case GameState.DEBATING:
                _cameraLook.ToggleCursor(true);
                ClueSlotsCanvasScript.ToggleVisibility?.Invoke(true);
                break;
            case GameState.SITTING:
                _currentlyPlaying = false;
                StartCoroutine(_cameraLook.ToggleSitting());
                InformationCanvasScript.ToggleVisibility?.Invoke(false);
                break;
            case GameState.TUTORIAL:
                TutorialCanvasScript.ToggleVisibility?.Invoke(true);
                break;
            case GameState.PLAYING:
                _cameraLook.ToggleCursor(true);
                CardGameCanvasScript.ToggleVisibility?.Invoke(true);
                DialogueCanvasScript.ToggleVisibility?.Invoke(false);
                if (!_currentlyPlaying) { Board.CreateNewVersionOfDeck?.Invoke(); _currentlyPlaying = true; }
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
                _interactWith.CastCursorRays();
                _playerMove.Move();
                break;
            case GameState.TUTORIAL:
                _interactWith.ListenForTutorial();
                break;
            case GameState.TALKING:
                _dialogueManager.ListenForNextDialogue();
                break;
            default:
                break;
        }
    }

    // Debating shouldn't become previous state, as to not confuse the dialogue state.
    private void ExitState()
    {
        switch (_currentState)
        {
            case GameState.WALKING:
                InformationCanvasScript.ToggleVisibility?.Invoke(false);
                _previousState = GameState.WALKING;
                break;
            case GameState.TUTORIAL:
                TutorialCanvasScript.ToggleVisibility?.Invoke(false);
                break;
            case GameState.DEBATING:
                _cameraLook.ToggleCursor(false);
                ClueSlotsCanvasScript.ToggleVisibility?.Invoke(false);
                break;
            case GameState.PLAYING:
                CardGameCanvasScript.ToggleVisibility?.Invoke(false);
                _previousState = GameState.PLAYING;
                break;
            default:
                break;
        }
    }
}
