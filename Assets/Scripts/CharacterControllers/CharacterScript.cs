using JetBrains.Annotations;
using System;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class CharacterScript : InteractableObject
{
    public string Character;
    public DialogueBranch nonQuestDialogue;
    private DialogueInvoker _invoker;

    public void Start()
    {
        base.SetCamera();
        base.SetOutline();
        _invoker = GetComponent<DialogueInvoker>();
        Character = this.gameObject.name;
    }

    // IDK how to feel about this code.
    public void TalkToCharacter()
    {
        try
        {
            var Q = (TalkToQuest)QuestManager.CurrentQuest?.Invoke("TalkTo");

            if (Q.Character == this.Character)
            {
                _invoker.SendDialogueBranch(Q.Dialogue);
                // QuestManager.CompleteQuest?.Invoke();
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
    }
}