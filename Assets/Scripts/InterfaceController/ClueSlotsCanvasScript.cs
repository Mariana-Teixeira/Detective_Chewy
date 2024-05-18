using System;
using UnityEngine;

public class ClueSlotsCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        ToggleVisibility += OnToggleVisibility;
    }

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();    
    }

    public void OnToggleVisibility(bool isVisible)
    {
        if(isVisible)
        {
            _canvasGroup.alpha = 1;
        }
        else
        {
            _canvasGroup.alpha = 0;
        }
    }
}
