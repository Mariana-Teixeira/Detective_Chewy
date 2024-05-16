using System;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class CharacterScript : InteractableObject
{
    public QuestManager QuestManager;
    public Board Board;
    public Target Target;

    private DialogueInvoker _invoker;

    public void Start()
    {
        base.SetCamera();
        base.SetOutline();
        _invoker = GetComponent<DialogueInvoker>();
    }

    public void TalkToCharacter()
    {
        var Q = QuestManager.CurrentQuest;

        if (Q.Target == this.Target)
        {
            TalkToCharacterAndCompleteQuest(Q);
        }
        else
        {
            LoadAlternativeDialogueBranch();
        }
    }

    private void TalkToCharacterAndCompleteQuest(Quest Q)
    {
        if (Board.ActiveTable == Q.ActiveTable && Board.CluesFound[Q.ClueArray] == Q.ClueFound)
        {
            Debug.Log("Quest Requirements Met");
            _invoker.SendDialogueBranch(Q.DialogueBranch);
            QuestManager.CompleteQuest?.Invoke();
        }
    }

    private void LoadAlternativeDialogueBranch()
    {
        throw new NotImplementedException();
    }
}