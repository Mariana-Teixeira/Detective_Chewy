using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(DialogueInvoker))]
public class Speaker : MonoBehaviour
{
    private DialogueInvoker _invoker;
    public DialogueBranch _dialogue;

    private void Start()
    {
        _invoker = GetComponent<DialogueInvoker>();
    }

    public void TalkTo()
    {
        PlayerStates.ChangeState?.Invoke(GameState.TALKING);
        _invoker.SendDialogueBranch(_dialogue);
    }
}