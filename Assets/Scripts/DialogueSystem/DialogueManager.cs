using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogueCanvasScript DialogueCanvas;

    private Queue<DialogueNode[]> BranchQueue;
    private DialogueNode[] CurrentBranch;
    private DialogueNode CurrentNode;
    private int DialogueIndex;
    private bool DialogueForQuest;
    private bool _isTalking;

    private void Start()
    {
        BranchQueue = new Queue<DialogueNode[]>();

        DialogueInvoker.SendDialogue += OnSendDialogue;
    }

    public void OnSendDialogue(DialogueNode[] nodes, bool isQuest)
    {
        DialogueForQuest = isQuest;
        BranchQueue.Enqueue(nodes);

        if (!_isTalking)
        {
            DialogueIndex = -1;
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        _isTalking = true;
        CurrentBranch = BranchQueue.Dequeue();
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
        }
    }

    public void IterateDialogue()
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
            if (BranchQueue.Count > 0)
            {
                CurrentBranch = BranchQueue.Dequeue();
                DialogueIndex = 0;
            }
            else
            {
                if (DialogueForQuest) QuestManager.CompleteQuest?.Invoke();
                EndDialogue();
                return;
            }
        }

        CurrentNode = CurrentBranch[DialogueIndex];
        DialogueCanvas.StartTypeWritterEffect(CurrentNode.DialogueText);
        CheckForNewTalkingState();
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
        _isTalking = false;
        CurrentBranch = null;
        DialogueForQuest = false;

        PlayerStates.ChangeState?.Invoke(GameState.WALKING);
    }
}
