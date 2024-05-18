using System;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;
    public static Action<string> UpdateCanvas;

    private Canvas _dialogueCanvas;
    public TMP_Text DialogueBox;

    private void Awake()
    {
        ToggleVisibility += OnToggleVisibility;
        UpdateCanvas += OnUpdateCanvas;
    }

    private void Start()
    {
        _dialogueCanvas = GetComponent<Canvas>();
    }

    public void OnToggleVisibility(bool isVisible)
    {
        _dialogueCanvas.enabled = isVisible;
    }

    public void OnUpdateCanvas(string text)
    {
        DialogueBox.text = text;
    }
}
