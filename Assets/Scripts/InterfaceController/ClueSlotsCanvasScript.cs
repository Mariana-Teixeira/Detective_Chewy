using System;
using UnityEngine;

public class ClueSlotsCanvasScript : MonoBehaviour
{
    public static Action<Thing> ToggleIcon;
    public static Action<bool> ToggleVisibility;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        ToggleVisibility += OnToggleVisibility;
        ToggleIcon += OnToggleIcon;
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

    public void OnToggleIcon(Thing thing)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var item = transform.GetChild(i);
            if (item.name == thing.ThingName)
            {
                item.gameObject.SetActive(true);
            }
        }

    }
}
