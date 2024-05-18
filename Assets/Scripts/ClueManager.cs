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
            _invoker.SendDialogueBranch(FinishedCollectingItems);
            QuestManager.CompleteQuest?.Invoke();
        }
    }

    // I don't know... I guess it's staying...
    public void OnInitQueue()
    {
        try
        {
            var Q = QuestManager.CurrentQuest?.Invoke("CollectThing") as CollectThingsQuest;
            Clues = new Queue<Thing>(Q.Things);
        }
        catch
        {
            Debug.Log("I didn't optimize this code, so enjoy this message.");
        }
    }
}