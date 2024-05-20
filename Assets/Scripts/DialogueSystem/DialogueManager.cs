using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private DialogueNode[] CurrentBranch;
    private DialogueNode CurrentNode;
    private int DialogueIndex;
    private bool DialogueForQuest;

    private void Start()
    {
        DialogueInvoker.SendDialogue += OnSendDialogue;
    }

    public void OnSendDialogue(DialogueNode[] nodes, bool isQuest)
    {
        DialogueForQuest = isQuest;

        if (CurrentBranch != null) // If the branch isn't empty, add whatever comes next to yourself.
        {
            var branch = CurrentBranch.Concat(nodes).ToArray();
            CurrentBranch = branch;
        }
        else
        {
            CurrentBranch = nodes;
        }


        DialogueIndex = -1;
        IterateDialogue();
    }

    public void ListenForNextDialogue()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IterateDialogue();
        }
    }

    public void ListenForClueSelect(string evidence)
    {
        if (evidence == CurrentNode.Evidence)
        {
            IterateDialogue();
        }
        else
        {
            EndDialogue();
            return;
        }
    }

    public void IterateDialogue()
    {
        DialogueIndex++;

        if (DialogueIndex >= CurrentBranch.Length)
        {
            if (DialogueForQuest) QuestManager.CompleteQuest?.Invoke();
            EndDialogue();
            return;
        }

        CurrentNode = CurrentBranch[DialogueIndex];
        CheckForNewTalkingState();
        DialogueCanvasScript.UpdateCanvas?.Invoke(CurrentNode.DialogueText);
    }

    private void CheckForNewTalkingState()
    {
        if (CurrentNode.Evidence != string.Empty)
        {
            PlayerStates.ChangeState?.Invoke(GameState.DEBATING);
        }
        else
        {
            PlayerStates.ChangeState?.Invoke(GameState.TALKING);
        }
    }

    public void EndDialogue()
    {
        CurrentBranch = null;
        DialogueForQuest = false;
        PlayerStates.PreviousState?.Invoke();
    }
}
