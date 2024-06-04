using System;
using UnityEngine;
using UnityEngine.UI;

public class CursorCanvas : MonoBehaviour
{
    public static Action<bool> ChangeCursor;

    Image _centerCursor;
    [SerializeField] Sprite _nonInteract, _interact;

    private void Awake()
    {
        _centerCursor = GetComponent<Image>();
    }

    private void Start()
    {
        ChangeCursor += OnChangeCursor;
    }


    public void OnChangeCursor(bool interactable)
    {
        if (interactable)
        {
            _centerCursor.sprite = _interact;
        }
        else
        {
            _centerCursor.sprite = _nonInteract;
        }
    }
}
