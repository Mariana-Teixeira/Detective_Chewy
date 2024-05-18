using System;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text DialogueText;
    private DialogueNode[] CurrentBranch;
    private DialogueNode CurrentNode;
    private int DialogueIndex;

    private void Start()
    {
        DialogueInvoker.SendDialogue += OnSendDialogue;
    }

    public void OnSendDialogue(DialogueNode[] nodes)
    {
        CurrentBranch = nodes;

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
    }

    public void IterateDialogue()
    {
        DialogueIndex++;

        if (DialogueIndex >= CurrentBranch.Length)
        {
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
        PlayerStates.PreviousState?.Invoke();
    }
}
