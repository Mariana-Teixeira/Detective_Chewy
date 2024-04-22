using System;
using System.Linq;
using UnityEngine;

public enum GameState { NULL, WALKING, SITTING, PLAYING, INSPECTING };
public class PlayerStates : MonoBehaviour
{
    public static Action<GameState> ChangeState;
    public static Action<GameObject> InspectItem;
    [SerializeField] GameObject cardGameUI;
    [SerializeField] GameObject inspectUI;
    [SerializeField] GameObject[] inspectItems;

    private int inspectItemNum = 0;
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
        InspectItem += OnInspect;
        OnChangeState(GameState.WALKING);
    }

    public GameState getGameState() 
    {
        return _currentState;
    }
    public void OnInspect(GameObject go) 
    {
        if (go.name.Contains("_0"))
        {
            inspectItems.ElementAt(0).SetActive(true);
            inspectItemNum = 0;
        }
        else if (go.name.Contains("_1"))
        {
            inspectItems.ElementAt(1).SetActive(true);
            inspectItemNum = 1;
        }
        else if (go.name.Contains("_2")) { 
            inspectItems.ElementAt(2).SetActive(true);
            inspectItemNum = 2;
        }
    }

    public void DisableInspectUI() 
    {
        inspectItems.ElementAt(inspectItemNum).SetActive(false);
        inspectUI.SetActive(false);
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
                DisableInspectUI();
                _cameraLook.ToggleCursor();
                cardGameUI.SetActive(false);
                break;
            case GameState.SITTING:
                StartCoroutine(_cameraLook.ToggleSitting());
                cardGameUI.SetActive(true);
                break;
            case GameState.INSPECTING:
                StartCoroutine(_cameraLook.ToggleInspecting());
                inspectUI.SetActive(true);
                _cameraLook.ToggleCursor();
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
            case GameState.INSPECTING:
                _interactWith.ListenForExitInspect();
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
            case GameState.INSPECTING:
                break;
            default:
                break;
        }
    }
}
