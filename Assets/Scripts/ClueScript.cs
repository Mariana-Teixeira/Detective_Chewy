using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class ClueScript : InteractableObject
{
    public QuestManager QuestManager;
    public Target Target;
    private DialogueInvoker _invoker;

    private void Start()
    {
        _invoker = GetComponent<DialogueInvoker>();
        base.setOutline();

        QuestManager.CompleteQuest += ShouldEnableClue;
    }

    public void ShouldEnableClue()
    {
        var Q = QuestManager.CurrentQuest;

        if (Q.Target == this.Target)
        {
            Debug.Log("Quest Requirements Met");
            this.gameObject.SetActive(true);
        }
    }

    public void GatherClue()
    {
        var Q = QuestManager.CurrentQuest;

        // _invoker.SendDialogueBranch(Q.DialogueBranch, true);
        QuestManager.CompleteQuest?.Invoke();
    }
}
