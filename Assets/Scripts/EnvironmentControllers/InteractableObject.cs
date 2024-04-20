using UnityEngine;

[RequireComponent (typeof(Outline))]
public class InteractableObject : MonoBehaviour
{
    private Outline _outline;
    [SerializeField] PlayerStates _playerState;

    private Camera _camera;



    private void Awake()
    {
        _camera = Camera.main;
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        _outline.OutlineWidth = 3;
        _outline.OutlineColor = Color.blue;
    }


    void OnMouseOver()
    {
        if (_playerState.getGameState() == GameState.WALKING)
        { 
            float dist = Vector3.Distance(_camera.transform.position, transform.position);
            //Debug.Log(dist);
            if (dist < 1.75)
            {
                _outline.enabled = true;
            }
            else {
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
        else
        {
        }
    }
}
