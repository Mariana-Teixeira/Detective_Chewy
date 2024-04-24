using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInvoker : MonoBehaviour
{
    public static Action<DialogueNode[]> SendDialogue;
    public List<DialogueBranch> Branch;
    public int BranchIndex;

    // Check which dialogue to start.
    public void SendDialogueBranch()
    {
        SendDialogue?.Invoke(Branch[BranchIndex].Nodes);
    }
}