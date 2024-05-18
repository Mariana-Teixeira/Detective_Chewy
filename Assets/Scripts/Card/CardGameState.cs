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
    First_Threshold,
    Second_Threshold,
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

    private PlayGameQuest _currentQuest;
    private DialogueInvoker _invoker;

    private void Awake()
    {
        ChangeGamePhase += ChangeState;
        UpdateQuest += OnUpdateQuest;
    }

    private void Start()
    {
        _invoker = GetComponent<DialogueInvoker>();
    }

    public void OnUpdateQuest(PlayGameQuest quest)
    {
        _currentQuest = quest;
    }

    public void ChangeState(GamePhase gamephase)
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
                Debug.Log("Start Game Dialogue");
                _invoker.SendDialogueBranch(_currentQuest.StartingGameDialogue);
                Board.CreateNewVersionOfDeck?.Invoke();
                break;
            case GamePhase.First_Threshold:
                Debug.Log("First Threshold Dialogue");
                _invoker.SendDialogueBranch(_currentQuest.FirstThresholdDialogue);
                break;
            case GamePhase.Second_Threshold:
                Debug.Log("Second Threshold Dialogue");
                _invoker.SendDialogueBranch(_currentQuest.SecondThresholdDialogue);
                break;
            case GamePhase.Win:
                Debug.Log("Win Game Dialogue");
                _invoker.SendDialogueBranch(_currentQuest.WinningGameDialogue);
                break;
            default:
                break;
        }
    }
}