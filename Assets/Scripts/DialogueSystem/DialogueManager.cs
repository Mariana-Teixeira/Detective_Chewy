using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogueCanvasScript DialogueCanvas;

    private DialogueNode[] CurrentBranch;
    private DialogueNode CurrentNode;
    private int DialogueIndex;
    private bool DialogueForQuest;
    private bool _isTalking;

    private void Start()
    {
        DialogueInvoker.SendDialogue += OnSendDialogue;
    }

    public void OnSendDialogue(DialogueNode[] nodes, bool isQuest)
    {
        DialogueForQuest = isQuest;
        CurrentBranch = nodes;

        if (!_isTalking)
        {
            DialogueIndex = -1;
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        _isTalking = true;
        IterateDialogueForward();
    }

    public void IterateDialogueBackward()
    {
        if (!DialogueCanvas.IsTyping)
        {
            MoveToPreviousNode();
        }
        else
        {
            DialogueCanvas.EndTypeWritterEffect();
        }
    }

    public void IterateDialogueForward()
    {
        if (!DialogueCanvas.IsTyping)
        {
            MoveToNextNode();
        }
        else
        {
            DialogueCanvas.EndTypeWritterEffect();
        }
    }

    public void MoveToNextNode()
    {
        DialogueIndex++;

        if (DialogueIndex >= CurrentBranch.Length)
        {
            if (DialogueForQuest) QuestManager.CompleteQuest?.Invoke();
            EndDialogue();
            return;
        }

        CurrentNode = CurrentBranch[DialogueIndex];
        DialogueCanvas.StartTypeWritterEffect(CurrentNode.DialogueText);
    }

    public void MoveToPreviousNode()
    {
        DialogueIndex--;

        if (DialogueIndex < 0) { DialogueIndex = 0; return; }

        CurrentNode = CurrentBranch[DialogueIndex];
        DialogueCanvas.StartTypeWritterEffect(CurrentNode.DialogueText);
    }

    public void EndDialogue()
    {
        _isTalking = false;
        CurrentBranch = null;
        DialogueForQuest = false;

        PlayerStates.ChangeState?.Invoke(GameState.WALKING);
    }
}
