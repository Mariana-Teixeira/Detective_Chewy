using System;
using System.Linq;
using UnityEngine;

public class InspectorCanvasScript : MonoBehaviour
{
    public static Action<GameObject> InspectItem;
    public static Action<bool> ToggleVisibility;

    private Canvas inspectorCanvas;

    [SerializeField] ClueScript[] _inspectItems;
    private int _inspectItemNum = 0;

    private void Start()
    {
        inspectorCanvas = GetComponent<Canvas>();

        InspectItem += OnInspectItem;
        ToggleVisibility += OnToggleVisibility;
    }

    public void OnInspectItem(GameObject item)
    {
        if (item.name.Contains("_0"))
        {
            _inspectItems.ElementAt(0).gameObject.SetActive(true);
            _inspectItemNum = 0;
        }
        else if (item.name.Contains("_1"))
        {
            _inspectItems.ElementAt(1).gameObject.SetActive(true);
            _inspectItemNum = 1;
        }
        else if (item.name.Contains("_2"))
        {
            _inspectItems.ElementAt(2).gameObject.SetActive(true);
            _inspectItemNum = 2;
        }
    }

    private void OnToggleVisibility(bool isVisible)
    {
        if (isVisible)
        {
            inspectorCanvas.enabled = true;
        }
        else
        {
            DisableInspectUI();
        }
    }

    public void DisableInspectUI()
    {
        inspectorCanvas.enabled = false;
    }
}
