using System;
using UnityEngine;

public enum GameState { NULL, WALKING, SITTING, PLAYING };
public class PlayerStates : MonoBehaviour
{
    public static Action<GameState> ChangeState;
    CameraLook _cameraLook;
    PlayerMove _playerMove;
    InteractWith _interactWith;
    GameState _currentState;

    private void Start()
    {
        _cameraLook = GetComponent<CameraLook>();
        _playerMove = GetComponent<PlayerMove>();
        _interactWith = GetComponent<InteractWith>();

        ChangeState += OnChangeState;
        OnChangeState(GameState.WALKING);
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
                _cameraLook.ToggleCursor();
                break;
            case GameState.SITTING:
                StartCoroutine(_cameraLook.ToggleSitting());
                break;
            case GameState.PLAYING:
                _cameraLook.ToggleCursor();
                break;
            default:
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
            default:
                break;
        }
    }

    private void ExitState()
    {
        switch (_currentState)
        {
            case GameState.WALKING:
                break;
            case GameState.SITTING:
                break;
            default:
                break;
        }
    }
}
