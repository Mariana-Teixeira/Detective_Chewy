using System;
using UnityEngine;

public class InterrogationCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;

    private Canvas _canvas;
    private bool _hasSeenTutorial;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        ToggleVisibility += OnToggleVisibility;
    }

    public void OnToggleVisibility(bool isVisible)
    {
        _canvas.enabled = isVisible;
    }
}
