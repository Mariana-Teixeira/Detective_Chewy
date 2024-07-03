using UnityEngine;

public class InterrogationManager : MonoBehaviour
{
    InterrogationCanvasScript _interrogationCanvas;
    DialogueManager _dialogueManager;

    private void Awake()
    {
        _dialogueManager = GetComponent<DialogueManager>();
    }
}
