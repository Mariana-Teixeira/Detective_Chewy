using System;
using UnityEngine;

public class CardGameCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;
    private Canvas _cardGameCanvas;

    private void Start()
    {
        _cardGameCanvas = GetComponent<Canvas>();

        ToggleVisibility += OnToggleVisibility;
    }

    public void OnToggleVisibility(bool isVisible)
    {
        _cardGameCanvas.enabled = isVisible;
    }
}
