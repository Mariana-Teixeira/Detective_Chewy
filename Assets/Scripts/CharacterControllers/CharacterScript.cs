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

    public void TalkToCharacter()
    {
        try
        {
            var Q = QuestManager.CurrentQuest?.Invoke("TalkTo") as TalkToQuest;

            if (Q != null)
            {
                _invoker.SendDialogueBranch(Q.Dialogue, true);
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