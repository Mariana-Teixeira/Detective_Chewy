using System;
using UnityEngine;

public enum TurnPhase
{
    Discard,
    Draw,
    Trade,
    Play,
}

public enum GamePhase
{
    Null,
    BoardSetup,
    Play,
    Tutorial,
    Start,
    Win,
    Lose
}

[RequireComponent(typeof(DialogueInvoker))]
public class CardGameState : MonoBehaviour
{
    public static Action<GamePhase> ChangeGamePhase;
    public static Action<PlayGameQuest> UpdateQuest;
    private GamePhase currentGamePhase;

    private CardLogic _logic;
    private Board _board;
    private Deck _deck;

    public TutorialCanvasScript Tutorial;

    private void Start()
    {
        ChangeGamePhase += OnChangeState;

        _logic = GetComponent<CardLogic>();
        _board = GetComponentInChildren<Board>();
        _deck = GetComponentInChildren<Deck>();
    }

    private void Update()
    {
        TickState();
    }

    public void OnChangeState(GamePhase gamephase)
    {
        if (gamephase == currentGamePhase) return;
        currentGamePhase = gamephase;
        EnterState();
    }

    public void EnterState()
    {
        switch(currentGamePhase)
        {
            case GamePhase.BoardSetup:
                _deck.gameObject.SetActive(true);
                _logic.Reposition();
                _logic.StartNewBoard();
                _board.CreateNewVersionOfDeck();
                break;
            case GamePhase.Play:
                Tutorial.ToggleVisibility(false);
                if (!_logic.TimerOn && _board.GetActiveTableLogic.UseTimer) StartCoroutine(_logic.StartTimer(_logic.CurrentMatchTime));
                break;
            case GamePhase.Tutorial:
                Tutorial.SetTutorial();
                Tutorial.ToggleVisibility(true);
                break;
            case GamePhase.Win:
                _logic.GameWon();
                _deck.gameObject.SetActive(false);
                QuestManager.CompleteQuest?.Invoke();
                PlayerStates.ChangeState?.Invoke(GameState.SITTING);
                break;
            case GamePhase.Lose:
                Debug.Log(_deck.gameObject);
                _logic.GameOver();
                _deck.gameObject.SetActive(false);
                PlayerStates.ChangeState?.Invoke(GameState.SITTING);
                break;
            default:
                break;
        }
    }

    public void TickState()
    {
        switch(currentGamePhase)
        {
            case GamePhase.Tutorial:
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) Tutorial.NextTutorialScreen();
                break;
        }
    }
}