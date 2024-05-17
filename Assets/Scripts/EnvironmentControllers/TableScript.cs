using System;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class TableScript : InteractableObject
{
    public Transform CardBodyPosition;
    [HideInInspector] public Transform CardCameraPosition;
    [HideInInspector] public Transform LookAtTarget;

    public string Game;
    public DialogueBranch nonQuestDialogue;
    private DialogueInvoker _invoker;

    public void Start()
    {
        base.SetCamera();
        base.SetOutline();
        LookAtTarget = this.transform.GetChild(0);
        CardCameraPosition = this.transform.GetChild(2);
        _invoker = GetComponent<DialogueInvoker>();
        Game = this.gameObject.name;
    }

    // I'm sorry, God.
    public bool PlayGame()
    {
        try
        {
            var Q = (PlayGameQuest)QuestManager.CurrentQuest?.Invoke("PlayGame");

            if (Q.Game == this.Game)
            {
                Board.CreateNewVersionOfDeck?.Invoke();
                return true;
            }
            else
            {
                _invoker.SendDialogueBranch(nonQuestDialogue);
            }
        }
        catch
        {
            _invoker.SendDialogueBranch(nonQuestDialogue);
        }

        return false;
    }
}