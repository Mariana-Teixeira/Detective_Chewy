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
    First_Threshold,
    Second_Threshold
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
            case GamePhase.First_Threshold:
                Debug.Log("First Threshold Dialogue");
                _invoker.SendDialogueBranch(_currentQuest.FirstThresholdDialogue);
                break;
            case GamePhase.Second_Threshold:
                Debug.Log("Second Threshold Dialogue");
                _invoker.SendDialogueBranch(_currentQuest.SecondThresholdDialogue);
                break;
            default:
                break;
        }
    }
}