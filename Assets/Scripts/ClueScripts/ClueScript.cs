using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class ClueScript : MonoBehaviour
{
    public string Clue;
    public DialogueBranch nonQuestDialogue;
    private DialogueInvoker _invoker;

    private void Awake()
    {
        _invoker = GetComponent<DialogueInvoker>();
        Clue = this.gameObject.name;
    }

    public void GatherClue()
    {
        var Q = QuestManager.CurrentQuest?.Invoke();
        var CCQ = Q as CollectThingsQuest;

        if (CCQ == null)
        {
            PlayerStates.ChangeState?.Invoke(GameState.TALKING);
            _invoker.SendDialogueBranch(nonQuestDialogue);
            return;
        }

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

            PlayerStates.ChangeState?.Invoke(GameState.TALKING);
            _invoker.SendDialogueBranch(item.Dialogue);

            ClueManager.FindClue?.Invoke(item);
            return true;
        }
        else
        {
            return false;
        }
    }
}