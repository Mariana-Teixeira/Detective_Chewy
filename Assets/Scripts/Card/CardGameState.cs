using System;
using System.Collections;
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
    public CardGameCanvasScript CardCanvas;

    public float _pauseTime = 2.0f;

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
                if (!_logic.TimerOn && _board.GetActiveTableLogic.UseTimer) _logic.StartTimer();
                break;
            case GamePhase.Tutorial:
                Tutorial.SetTutorial();
                Tutorial.ToggleVisibility(true);
                break;
            case GamePhase.Win:
                StartCoroutine("WinGameCoroutine");
                break;
            case GamePhase.Lose:
                StartCoroutine("LoseGameCoroutine");
                break;
            default:
                break;
        }
    }

    public IEnumerator LoseGameCoroutine()
    {
        CardCanvas.CallLoseCanvas();
        CardCanvas.ToggleStatusWindows(true);
        _deck.gameObject.SetActive(false);

        _logic.StopTimer();
        _logic.ResetGame();

        yield return new WaitForSeconds(_pauseTime);

        CardCanvas.ToggleStatusWindows(false);
        PlayerStates.ChangeState?.Invoke(GameState.SITTING);
    }

    public IEnumerator WinGameCoroutine()
    {
        _logic.StopTimer();

        CardCanvas.CallWinCanvas();
        CardCanvas.ToggleStatusWindows(true);
        _deck.gameObject.SetActive(false);

        yield return new WaitForSeconds(_pauseTime);

        _logic.ResetGame();
        _board.SetNextActiveTable();
        CardCanvas.ToggleStatusWindows(false);
        QuestManager.CompleteQuest?.Invoke();
        PlayerStates.ChangeState?.Invoke(GameState.SITTING);
    }

    public void TickState()
    {
        switch(currentGamePhase)
        {
            case GamePhase.Play:
                if (_board.GetActiveTableLogic.UseTimer) _logic.TickTimer();
                break;
        }
    }
}