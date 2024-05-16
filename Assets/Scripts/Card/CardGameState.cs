using System;
using UnityEngine;
using UnityEngine.Scripting;

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

[RequireComponent(typeof(DialogueInvoker))]
public class CardGameState : MonoBehaviour
{
    private DialogueInvoker _invoker;
    public DialogueBranch[,] branches;
    
    private GamePhase currentGamePhase;
    public static Action<GamePhase, int> ChangeGamePhase;

    private void Start()
    {
        ChangeGamePhase += EnterState;
    }

    public void EnterState(GamePhase gamephase, int activeTable)
    {
        currentGamePhase = gamephase;

        switch(currentGamePhase)
        {
            case GamePhase.Start:
                break;
            case GamePhase.First_Threshold:
                break;
            case GamePhase.Second_Threshold:
                break;
            case GamePhase.Win:
                QuestManager.CompleteQuest?.Invoke();
                break;
            case GamePhase.Lose:
                break;
        }
    }
}