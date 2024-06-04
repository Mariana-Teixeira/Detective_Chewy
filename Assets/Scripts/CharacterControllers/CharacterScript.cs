using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class CharacterScript : MonoBehaviour
{
    public string Character;
    public DialogueBranch nonQuestDialogue;
    private DialogueInvoker _invoker;

    public GameObject Exclamation;

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

    public void ToggleExclamation()
    {
        var Quest = ReturnQuest();
        if (Quest != null)
        {
            Exclamation.SetActive(true);
        }
        else
        {
            Exclamation.SetActive(false);
        }
    }

    public void TalkToCharacter()
    {
        var Quest = ReturnQuest();
        if (Quest != null)
        {
            _invoker.SendDialogueBranch(Quest.Dialogue, true);
        }
        else
        {
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
            return TTQ;
        }
        return null;
    }
}