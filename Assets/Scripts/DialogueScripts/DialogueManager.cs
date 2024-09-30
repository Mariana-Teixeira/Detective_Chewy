using System;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogueCanvasScript DialogueCanvas;

    private DialogueNode[] _currentBranch;
    private int DialogueIndex;
    private bool DialogueForQuest;
    private bool _isTalking;

    private bool _stopDialogueEnd;

    public DialogueNode CurrentNode
    {
        get
        {
            return _currentBranch[DialogueIndex];
        }
        set
        {
            _currentBranch[DialogueIndex] = value;
        }
    }

    private void Start()
    {
        DialogueInvoker.SendDialogue += OnSendDialogue;
        PlayerStates.ChangeState += OnChangeState;
    }

    private void OnChangeState(GameState state)
    {
        if (state == GameState.INTERROGATING)
        {
            _stopDialogueEnd = true;
        }
        else
        {
            _stopDialogueEnd = false;
        }
    }

    public void OnSendDialogue(DialogueNode[] nodes, bool isQuest)
    {
        DialogueForQuest = isQuest;

        if (!_isTalking)
        {
            _currentBranch = nodes;
            StartDialogue();
        }
        else
        {
            MergeDialogue(nodes);
        }

        DialogueCanvas.UpdateArrows(_currentBranch, DialogueIndex, _stopDialogueEnd);
    }

    // Assumes we won't stop the dialogue by the end of a interrogation.
    private void MergeDialogue(DialogueNode[] mergeNodes)
    {
        _stopDialogueEnd = false;

        var newBranch = new DialogueNode[_currentBranch.Length + mergeNodes.Length];
        _currentBranch.CopyTo(newBranch, 0);
        mergeNodes.CopyTo(newBranch, _currentBranch.Length);

        _currentBranch = newBranch;
    }

    public void StartDialogue()
    {
        _isTalking = true;
        DialogueIndex = 0;
        UpdateNode();
    }

    public void IterateDialogueBackward()
    {
        if (!DialogueCanvas.IsTyping)
        {
            MoveToPreviousNode();
            DialogueCanvas.UpdateArrows(_currentBranch, DialogueIndex, _stopDialogueEnd);
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
            DialogueCanvas.UpdateArrows(_currentBranch, DialogueIndex, _stopDialogueEnd);
        }
        else
        {
            DialogueCanvas.EndTypeWritterEffect();
        }
    }

    public void UpdateNode(DialogueNode node)
    {
        CurrentNode = node;
        DialogueCanvas.EndTypeWritterEffect();
        DialogueCanvas.StartTypeWritterEffect(CurrentNode.DialogueText);
        DialogueCanvas.EndSound();
        DialogueCanvas.StartSound(CurrentNode.DialogueSound);
    }

    public void MoveToNextNode()
    {
        DialogueIndex++;

        if (DialogueIndex >= _currentBranch.Length)
        {
            CheckDialogueEnd();
            return;
        }

        UpdateNode();
    }

    public void UpdateNode()
    {
        CurrentNode = _currentBranch[DialogueIndex];
        DialogueCanvas.StartTypeWritterEffect(CurrentNode.DialogueText);
        DialogueCanvas.EndSound();
        DialogueCanvas.StartSound(CurrentNode.DialogueSound);
    }

    public void CheckDialogueEnd()
    {
        if (_stopDialogueEnd)
        {
            DialogueIndex = _currentBranch.Length - 1;
        }
        else
        {
            DialogueCanvas.EndSound();
            EndDialogue();

            // Father, I do sin. I sin hard.
            if (DialogueForQuest) { QuestManager.CompleteQuest?.Invoke(); DialogueForQuest = false; }
        }
    }

    public void MoveToPreviousNode()
    {
        DialogueIndex--;

        if (DialogueIndex < 0) { DialogueIndex = 0; return; }

        UpdateNode();
    }

    public void EndDialogue()
    {
        _isTalking = false;
        _currentBranch = null;

        PlayerStates.ChangeState?.Invoke(GameState.WALKING);
    }
}
