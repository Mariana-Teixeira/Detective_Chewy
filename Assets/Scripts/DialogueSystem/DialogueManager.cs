using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Canvas DialogueCanvas;
    public TMP_Text DialogueText;
    private DialogueNode[] CurrentBranch;
    private int DialogueIndex;
    private bool DialogueForQuest;

    private void Start()
    {
        DialogueInvoker.SendDialogue += OnSendDialogue;
    }

    public void OnSendDialogue(DialogueNode[] nodes, bool QuestDialogue)
    {
        CurrentBranch = nodes;
        DialogueForQuest = QuestDialogue;

        DialogueIndex = 0;
        StartDialogue();
        IterateDialogue();
    }

    public void ListenForNextDialogue()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            IterateDialogue();
        }
    }

    public void IterateDialogue()
    {
        if (DialogueIndex >= CurrentBranch.Length)
        {
            EndDialogue();
        }
        else
        {
            DisplayText(CurrentBranch[DialogueIndex].DialogueText);
            DialogueIndex++;
        }
    }

    public void StartDialogue()
    {
        DialogueCanvas.enabled = true;
    }

    public void EndDialogue()
    {
        DialogueCanvas.enabled = false;
        CurrentBranch = null;
        PlayerStates.PreviousState?.Invoke();

        if (DialogueForQuest)
        {
            QuestManager.CompleteQuest?.Invoke();
        }
    }

    public void DisplayText(string text)
    {
        DialogueText.text = text;
    }

    public void PlaySound()
    {

    }
}
