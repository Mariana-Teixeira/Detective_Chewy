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
    private GamePhase currentGamePhase;

    public PlayGameQuest Quest;

    private DialogueInvoker _invoker;

    private void Awake()
    {
        ChangeGamePhase += EnterState;
        Board.CreateNewVersionOfDeck = () => Quest = (PlayGameQuest)QuestManager.CurrentQuest?.Invoke("PlayGame");
    }

    private void Start()
    {
        _invoker = GetComponent<DialogueInvoker>();
    }

    public void EnterState(GamePhase gamephase)
    {
        currentGamePhase = gamephase;

        switch(currentGamePhase)
        {
            case GamePhase.Start:
                _invoker.SendDialogueBranch(Quest.StartingGameDialogue);
                break;
            case GamePhase.First_Threshold:
                _invoker.SendDialogueBranch(Quest.FirstThresholdDialogue);
                break;
            case GamePhase.Second_Threshold:
                _invoker.SendDialogueBranch(Quest.SecondThresholdDialogue);
                break;
            case GamePhase.Win:
                _invoker.SendDialogueBranch(Quest.WinningGameDialogue);
                break;
            default:
                break;
        }
    }
}