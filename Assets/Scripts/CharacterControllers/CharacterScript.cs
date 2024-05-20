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
        var Q = QuestManager.CurrentQuest?.Invoke("TalkTo");
        var TTQ = Q as TalkToQuest;

        if(TTQ != null && TTQ.Character == this.Character)
        {
            _invoker.SendDialogueBranch(TTQ.Dialogue, true);
        }
        else
        {
            _invoker.SendDialogueBranch(nonQuestDialogue);
        }
    }
}