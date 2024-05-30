using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using UnityEngine;

public class DialogueInvoker : MonoBehaviour
{
    public static Action<DialogueNode[], bool> SendDialogue;

    // Check which dialogue to start.
    public void SendDialogueBranch(DialogueBranch branch, bool isQuest = false)
    {
        if (branch == null) Debug.LogError("Branch is Null");
        PlayerStates.ChangeState?.Invoke(GameState.TALKING);
        SendDialogue?.Invoke(branch.Nodes, isQuest);
    }
}