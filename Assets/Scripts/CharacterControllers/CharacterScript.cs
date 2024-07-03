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
        var TalkTo = ReturnTalkToQuest();
        var Interrogate = ReturnInterrogateQuest();

        if (TalkTo || Interrogate != null)
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
        var TalkTo = ReturnTalkToQuest();
        var Interrogate = ReturnInterrogateQuest();

        if (TalkTo != null)
        {
            PlayerStates.ChangeState?.Invoke(GameState.TALKING);
            _invoker.SendDialogueBranch(TalkTo.Dialogue, true);
        }
        else if (Interrogate != null)
        {
            PlayerStates.ChangeState?.Invoke(GameState.INTERROGATING);
            _invoker.SendDialogueBranch(Interrogate.Branch, false);
            Debug.Log("Interrogation Time");
        }
        else
        {
            PlayerStates.ChangeState?.Invoke(GameState.TALKING);
            _invoker.SendDialogueBranch(nonQuestDialogue);
        }
    }

    public TalkToQuest ReturnTalkToQuest()
    {
        var Q = QuestManager.CurrentQuest?.Invoke();
        var TTQ = Q as TalkToQuest;

        if (TTQ == null) { return null; }

        if (TTQ.Character == this.Character)
        {
            return TTQ;
        }
        else
        {
            return null;
        }
    }

    public InterrogateQuest ReturnInterrogateQuest()
    {
        var Q = QuestManager.CurrentQuest?.Invoke();
        var IQ = Q as InterrogateQuest;

        if (IQ == null) { return null; }

        if (IQ.Character == this.Character)
        {
            return IQ;
        }
        else
        {
            return null;
        }
    }
}