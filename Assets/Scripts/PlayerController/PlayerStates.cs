using System;
using UnityEngine;

public enum GameState { NULL, WALKING, SITTING, PLAYING, INTERROGATING, TALKING, MENU };
public class PlayerStates : MonoBehaviour
{
    public static Action PreviousState;
    public static Action<GameState> ChangeState;

    CameraLook _cameraLook;
    PlayerMove _playerMove;
    InteractWith _interactWith;
    GameState _currentState;

    [SerializeField] DialogueManager DialogueManager;
    [SerializeField] MenuManager MenuManager;

    private void Start()
    {
        _cameraLook = GetComponent<CameraLook>();
        _playerMove = GetComponent<PlayerMove>();
        _interactWith = GetComponent<InteractWith>();

        ChangeState += OnChangeState;

        OnChangeState(GameState.WALKING);
    }

    public GameState GetGameState() 
    {
        return _currentState;
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
                break;
            case GameState.TALKING:
                _cameraLook.ToggleCursor(true);
                DialogueCanvasScript.ToggleVisibility?.Invoke(true);
                break;
            case GameState.INTERROGATING:
                _cameraLook.ToggleCursor(true);
                DialogueCanvasScript.ToggleVisibility?.Invoke(true);
                InterrogationCanvasScript.ToggleVisibility?.Invoke(true);
                break;
            case GameState.SITTING:
                StartCoroutine(_cameraLook.ToggleSitting());
                InformationCanvasScript.ToggleVisibility?.Invoke(false);
                break;
            case GameState.PLAYING:
                _cameraLook.ToggleCursor(true);
                CardGameCanvasScript.ToggleVisibility?.Invoke(true);
                CardGameState.ChangeGamePhase(GamePhase.BoardSetup);
                break;
            case GameState.MENU:
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
                _interactWith.CastCursorRays();
                _playerMove.Move();
                if (MenuManager.ListenForToggleMenu) MenuManager.ToggleCanvas(_currentState);
                break;
            case GameState.MENU:
                if (MenuManager.ListenForToggleMenu) MenuManager.ToggleCanvas(_currentState);
                break;
            default:
                break;
        }
    }

    private void ExitState()
    {
        switch (_currentState)
        {
            case GameState.WALKING:
                InformationCanvasScript.ToggleVisibility?.Invoke(false);
                break;
            case GameState.TALKING:
                DialogueCanvasScript.ToggleVisibility?.Invoke(false);
                break;
            case GameState.INTERROGATING:
                DialogueCanvasScript.ToggleVisibility?.Invoke(false);
                InterrogationCanvasScript.ToggleVisibility?.Invoke(false);
                break;
            case GameState.PLAYING:
                CardGameCanvasScript.ToggleVisibility?.Invoke(false);
                break;
            case GameState.MENU:
                _cameraLook.ToggleCursor(false);
                break;
            default:
                break;
        }
    }
}
