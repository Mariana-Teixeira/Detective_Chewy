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
    public void GatherClue()
    {
        var Q = QuestManager.CurrentQuest?.Invoke();
        var CCQ = Q as CollectThingsQuest;

        if (CCQ == null) { return; }

        for (int i = 0; i < CCQ.Things.Length; i++)
        {
            var item = CCQ.Things[i];
            CheckClue(item);
        }
    }

    public bool CheckClue(Thing item)
    {
        if (item.ThingName == this.Clue)
        {
            this.gameObject.SetActive(false);
            _invoker.SendDialogueBranch(item.Dialogue);
            ClueManager.FindClue?.Invoke(item);
            ClueSlotsCanvasScript.ToggleIcon?.Invoke(item);
            return true;
        }
        else
        {
            return false;
        }
    }
}
