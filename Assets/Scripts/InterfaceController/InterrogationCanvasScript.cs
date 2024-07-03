using System;
using UnityEngine;

public class InterrogationCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;

    private Canvas _canvas;
    private GameObject _clueGroup;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _clueGroup = this.transform.GetChild(1).gameObject;
    }

    private void Start()
    {
        ToggleVisibility += OnToggleVisibility;
    }

    public void OnToggleVisibility(bool isVisible)
    {
        _canvas.enabled = isVisible;
    }

    public void OnToggleIcons(string[] clues)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var item = transform.GetChild(i);
            if (item.name == clues[i])
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
