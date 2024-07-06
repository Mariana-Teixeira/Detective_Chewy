using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class CharacterScript : MonoBehaviour
{
    public string Character;
    public DialogueBranch nonQuestDialogue;
    private DialogueInvoker _invoker;

    public GameObject QuestMarker;

    private void Awake()
    {
        _invoker = GetComponent<DialogueInvoker>();
        Character = this.gameObject.name;
    }

    public void Start()
    {
        ToggleExclamation();
        QuestManager.CompleteQuest += ToggleExclamation;
    }

    // You have to admit... this is code.
    public void ToggleExclamation()
    {
        var TalkTo = ReturnQuest();
        if (TalkTo != null) QuestMarker.SetActive(true);
        else QuestMarker.SetActive(false);
    }

    public void TalkToCharacter()
    {
        var TalkTo = ReturnQuest();

        if (TalkTo != null)
        {
            if (TalkTo.Interrogate)
            {
                PlayerStates.ChangeState?.Invoke(GameState.INTERROGATING);
                DeductionFrameScript.GetQuest?.Invoke(TalkTo);
                var instanceBranch = Instantiate(TalkTo.Dialogue);
                _invoker.SendDialogueBranch(instanceBranch, false);
            }
            else
            {
                PlayerStates.ChangeState?.Invoke(GameState.TALKING);
                _invoker.SendDialogueBranch(TalkTo.Dialogue, true);
            }
        }
        else
        {
            PlayerStates.ChangeState?.Invoke(GameState.TALKING);
            _invoker.SendDialogueBranch(nonQuestDialogue);
        }
    }

    public TalkToQuest ReturnQuest()
    {
        var Q = QuestManager.CurrentQuest?.Invoke();
        var TTQ = Q as TalkToQuest;

        if (TTQ == null) { return null; }

        if (TTQ.Character == this.Character)
        {
            QuestMarker.SetActive(false);
            return TTQ;
        }
        else
        {
            return null;
        }
    }
}