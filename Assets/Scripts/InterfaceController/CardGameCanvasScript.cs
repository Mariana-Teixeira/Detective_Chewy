using System;
using UnityEngine;

public class CardGameCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;
    private Canvas _cardGameCanvas;

    private void Awake()
    {
        ToggleVisibility += OnToggleVisibility;
    }

    private void Start()
    {
        _cardGameCanvas = GetComponent<Canvas>();
    }

    public void OnToggleVisibility(bool isVisible)
    {
        _cardGameCanvas.enabled = isVisible;
    }
}
