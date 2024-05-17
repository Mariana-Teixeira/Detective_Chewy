using System;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class ClueScript : InteractableObject
{
    public string Clue;
    public DialogueBranch nonQuestDialogue;
    private DialogueInvoker _invoker;

    private void Start()
    {
        base.SetCamera();
        base.SetOutline();
        _invoker = GetComponent<DialogueInvoker>();
        Clue = this.gameObject.name;
    }

    // I don't enjoy it either, let me live. I'm thinking!
    public bool GatherClue()
    {
        try
        {
            var Q = (CollectThingQuest)QuestManager.CurrentQuest?.Invoke("CollectThing");

            if (Q.Clue == this.Clue)
            {
                this.gameObject.SetActive(false);
                QuestManager.CompleteQuest?.Invoke();
                return true;
            }
            else
            {
                _invoker.SendDialogueBranch(nonQuestDialogue);
            }
        }
        catch
        {
            _invoker.SendDialogueBranch(nonQuestDialogue);
        }

        return false;
    }
}
