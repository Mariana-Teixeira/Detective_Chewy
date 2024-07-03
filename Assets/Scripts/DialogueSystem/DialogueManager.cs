using System;
using System.Collections.Generic;
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
        _currentBranch = nodes;

        if (!_isTalking)
        {
            DialogueIndex = -1;
            StartDialogue();
        }
    }

    public void ListenForNextDialogue()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IterateDialogueForward();
        }
        if (Input.GetMouseButton(1))
        {
            IterateDialogueBackward();
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

    public void UpdateNode(DialogueNode node)
    {
        CurrentNode = node;
        DialogueCanvas.EndTypeWritterEffect();
        DialogueCanvas.StartTypeWritterEffect(CurrentNode.DialogueText);
    }

    public void MoveToNextNode()
    {
        DialogueIndex++;

        if (DialogueIndex >= _currentBranch.Length)
        {
            CheckDialogueEnd();
            return;
        }

        CurrentNode = _currentBranch[DialogueIndex];
        DialogueCanvas.StartTypeWritterEffect(CurrentNode.DialogueText);
    }

    public void CheckDialogueEnd()
    {
        if (_stopDialogueEnd)
        {
            DialogueIndex = _currentBranch.Length;
        }
        else
        {
            if (DialogueForQuest) QuestManager.CompleteQuest?.Invoke();
            EndDialogue();
        }
    }

    public void MoveToPreviousNode()
    {
        DialogueIndex--;

        if (DialogueIndex < 0) { DialogueIndex = 0; return; }

        CurrentNode = _currentBranch[DialogueIndex];
        DialogueCanvas.StartTypeWritterEffect(CurrentNode.DialogueText);
    }

    public void EndDialogue()
    {
        _isTalking = false;
        _currentBranch = null;
        DialogueForQuest = false;

        PlayerStates.ChangeState?.Invoke(GameState.WALKING);
    }
}
