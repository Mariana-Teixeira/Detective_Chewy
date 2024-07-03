using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueInvoker))]
public class ClueManager : MonoBehaviour
{
    public static Action InitQueue;
    public static Action<Thing> FindClue;

    public DialogueBranch FinishedCollectingItems;
    private DialogueInvoker _invoker;
    private Queue<Thing> Clues;

    private void Awake()
    {
        _invoker = GetComponent<DialogueInvoker>();
    }

    private void Start()
    {
        FindClue += OnFindClue;
        InitQueue += OnInitQueue;
    }

    public void OnFindClue(Thing thing)
    {
        Clues.Dequeue();
        ShouldEndQuest();
    }

    public void ShouldEndQuest()
    {
        if (Clues.Count <= 0)
        {
            PlayerStates.ChangeState?.Invoke(GameState.TALKING);
            _invoker.SendDialogueBranch(FinishedCollectingItems, true);
        }
    }

    public void OnInitQueue()
    {
        var Q = QuestManager.CurrentQuest?.Invoke();
        var CTQ = Q as CollectThingsQuest;
        if (CTQ != null) Clues = new Queue<Thing>(CTQ.Things);
    }
}