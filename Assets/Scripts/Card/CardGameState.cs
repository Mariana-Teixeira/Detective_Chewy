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
    Start,
    Win,
    Lose
}

// This one has something special... like milk gone bad.
[RequireComponent(typeof(DialogueInvoker))]
public class CardGameState : MonoBehaviour
{
    public static Action<GamePhase> ChangeGamePhase;
    public static Action<PlayGameQuest> UpdateQuest;
    private GamePhase currentGamePhase;

    private CardLogic _logic;
    private Board _board;
    private Deck _deck;

    private void Start()
    {
        ChangeGamePhase += OnChangeState;

        _logic = GetComponent<CardLogic>();
        _board = GetComponentInChildren<Board>();
        _deck = GetComponentInChildren<Deck>();
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
            case GamePhase.Start:
                _deck.gameObject.SetActive(true);
                _logic.Reposition();
                _logic.StartNewBoard();
                _board.CreateNewVersionOfDeck();
                StartCoroutine(_logic.StartTimer(_logic.CurrentMatchTime));
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
}