using System;
using UnityEngine;

[RequireComponent (typeof(Outline))]
public class InteractableObject : MonoBehaviour
{
    private Outline _outline;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = true;
        _outline.OutlineWidth = 5;
        _outline.OutlineColor = Color.blue;
    }

    private void OnMouseOver()
    {
        _outline.OutlineColor = Color.white;
    }

    private void OnMouseExit()
    {
        _outline.OutlineColor = Color.blue;
    }
}
