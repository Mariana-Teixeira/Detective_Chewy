using System;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class TableScript : MonoBehaviour
{
    public Transform CardBodyPosition;
    [HideInInspector] public Transform CardCameraPosition;
    [HideInInspector] public Transform LookAtTarget;

    [SerializeField] Board board;

    public string Game;
    public DialogueBranch nonQuestDialogue;
    private DialogueInvoker _invoker;

    public GameObject Exclamation;

    private void Awake()
    {
        Game = this.gameObject.name;
        LookAtTarget = this.transform.GetChild(0);
        CardCameraPosition = this.transform.GetChild(2);
        _invoker = GetComponent<DialogueInvoker>();
    }

    private void Start()
    {
        ToggleExclamation();
        QuestManager.CompleteQuest += ToggleExclamation;
    }

    // I know, I know. I just need it to work for Friday!
    public void ToggleExclamation()
    {
        var Quest = ReturnQuest();
        if(Quest != null)
        {
            Exclamation.SetActive(true);
        }
        else
        {
            Exclamation.SetActive(false);
        }
    }

    // I'm sorry, God.
    public bool PlayGame()
    {
        board.OnCreateNewVersionOfDeck();
        var Quest = ReturnQuest();
        if(Quest != null)
        {
            CardGameState.UpdateQuest?.Invoke(Quest);
            return true;
        }
        else
        {
            _invoker.SendDialogueBranch(nonQuestDialogue);
            return false;
        }
    }

    public PlayGameQuest ReturnQuest()
    {
        var Q = QuestManager.CurrentQuest?.Invoke();
        var PGQ = Q as PlayGameQuest;

        if (PGQ == null) { return null; }

        if (PGQ.Game == this.Game)
        {
            return PGQ;
        }
        return null;
    }
}