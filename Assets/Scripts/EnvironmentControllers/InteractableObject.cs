using UnityEngine;

[RequireComponent (typeof(Outline))]
public class InteractableObject : MonoBehaviour
{
    private Outline _outline;
    public Color _outlineColor;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = true;
        _outline.OutlineWidth = 5;
        _outline.OutlineColor = _outlineColor;
    }
}
