using System;
using UnityEngine;

public class InterrogationCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;

    private CanvasGroup _canvas;

    private void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        ToggleVisibility += OnToggleVisibility;
    }

    public void OnToggleVisibility(bool isVisible)
    {
        if (isVisible) _canvas.alpha = 1f;
        else _canvas.alpha = 0f;
    }
}
