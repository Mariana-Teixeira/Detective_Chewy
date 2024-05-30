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
        FindClue += OnFindClue;
        InitQueue += OnInitQueue;

        _invoker = GetComponent<DialogueInvoker>();
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