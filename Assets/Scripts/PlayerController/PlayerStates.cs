using System;
using UnityEngine;

public enum GameState { OVERWORLD, SITTING, CARD };
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
            case GameState.OVERWORLD:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case GameState.SITTING:
                _cameraLook.StartCoroutine("SittingAnimation");
                Cursor.lockState = CursorLockMode.Confined;
                break;
            default:
                break;
        }
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case GameState.OVERWORLD:
                _cameraLook.RotateWithMouse();
                _interactWith.CastInteractionRays();
                _playerMove.Move();
                break;
            case GameState.SITTING:
                break;
            default:
                break;
        }
    }

    private void ExitState()
    {
        switch (_currentState)
        {
            case GameState.OVERWORLD:
                break;
            case GameState.SITTING:
                _cameraLook.StopCoroutine("SittingAnimation");
                break;
            default:
                break;
        }
    }
}
