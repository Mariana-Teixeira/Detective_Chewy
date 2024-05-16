using UnityEngine;

[RequireComponent (typeof(Outline))]
public class InteractableObject : MonoBehaviour
{
    private Outline _outline;
    [SerializeField] PlayerStates _playerState;

    private Camera _gameCamera;

    public void SetCamera()
    {
        _gameCamera = Camera.main;
    }

    public void SetOutline() 
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        _outline.OutlineWidth = 3;
        _outline.OutlineColor = Color.blue;
    }

    void OnMouseOver()
    {
        if (_playerState.GetGameState() == GameState.WALKING)
        { 
            float distance = Vector3.Distance(_gameCamera.transform.position, transform.position);

            if (distance < 2.25)
            {
                _outline.enabled = true;
            }
            else
            {
                _outline.enabled = false;
            }
        }
    }

    void OnMouseExit()
    {
        if (_outline.enabled)
        {
            _outline.enabled = false;
        }
    }
}
