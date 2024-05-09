using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class CharacterScript : InteractableObject
{
    public QuestManager QuestManager;
    public Board Board;
    public Target Target;

    private DialogueInvoker _invoker;

    private void Start()
    {
        _invoker = GetComponent<DialogueInvoker>();
        base.setOutline();
    }

    // Hard-Coded
    public bool TalkToCharacter()
    {
        var Q = QuestManager.CurrentQuest;
        if (Q.Target != this.Target) return false;

        if (Board.ActiveTable == Q.ActiveTable && Board.CluesFound[Q.ClueArray] == Q.ClueFound)
        {
            Debug.Log("Quest Requirements Met");
            _invoker.SendDialogueBranch(Q.DialogueBranch, true);
            return true;
        }

        Debug.LogError("Couldn't find corresponding Quest.");
        return false;
    }
}