using System;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class TableScript : MonoBehaviour
{
    public Transform CardBodyPosition;
    [HideInInspector] public Transform CardCameraPosition;
    [HideInInspector] public Transform LookAtTarget;

    public string Game;
    public DialogueBranch nonQuestDialogue;
    private DialogueInvoker _invoker;

    public GameObject QuestMarker;

    private void Awake()
    {
        Game = this.gameObject.name;
        LookAtTarget = this.transform.GetChild(0);
        CardCameraPosition = this.transform.GetChild(2);
        _invoker = GetComponent<DialogueInvoker>();
    }

    private void Start()
    {
        QuestManager.CompleteQuest += ToggleExclamation;

        ToggleExclamation();
    }

    // I sin. I keep sinning.
    public void ToggleExclamation()
    {
        var Quest = ReturnQuest();
        if(Quest != null)
        {
            QuestMarker.SetActive(true);
        }
        else
        {
            QuestMarker.SetActive(false);
        }
    }

    // I'm sorry, God.
    public bool PlayGame()
    {
        var Quest = ReturnQuest();
        if(Quest != null)
        {
            CardGameState.UpdateQuest?.Invoke(Quest);
            return true;
        }
        else
        {
            PlayerStates.ChangeState?.Invoke(GameState.TALKING);
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
            QuestMarker.SetActive(false);
            return PGQ;
        }
        return null;
    }
}