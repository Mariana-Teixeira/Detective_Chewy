using System;
using UnityEngine;

public class DialogueCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;
    private Canvas _dialogueCanvas;

    private void Awake()
    {
        ToggleVisibility += OnToggleVisibility;
    }

    private void Start()
    {
        _dialogueCanvas = GetComponent<Canvas>();
    }

    public void OnToggleVisibility(bool isVisible)
    {
        _dialogueCanvas.enabled = isVisible;
    }
}
