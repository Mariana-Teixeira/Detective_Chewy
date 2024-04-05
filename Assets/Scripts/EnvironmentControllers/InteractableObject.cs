using UnityEngine;

[RequireComponent (typeof(Outline))]
public class InteractableObject : MonoBehaviour
{
    private Outline _outline;
    [SerializeField] PlayerStates _playerState;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        _outline.OutlineWidth = 3;
        _outline.OutlineColor = Color.blue;
    }


    void OnMouseOver()
    {
        if (_playerState.getGameState() == GameState.WALKING) 
        { 
        _outline.enabled = true;
        }
    }

    void OnMouseExit()
    {
        if (_outline.enabled)
        {
            _outline.enabled = false;
        }
        else
        {
        }
    }
}
