using UnityEngine;

public class ChairScript : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject infoField;
    [SerializeField] GameObject textGameObject;

    private bool _infoOn = false;
    private Camera _camera;

    private Vector3 _initialPos;
    void Start()
    {
        _camera = Camera.main;
        _initialPos = textGameObject.transform.position;
        infoField.SetActive(false);


    }
    void Update()
    {
        //Turn interactor towards player so its always readable
        if (_infoOn) {
            textGameObject.transform.LookAt(_camera.transform);
            textGameObject.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
            float y = Mathf.PingPong(Time.time, 1) * 0.005f -0.0025f ;
            textGameObject.transform.position = textGameObject.transform.position + new Vector3(0,y, 0);
        }

    }

    public void Interact()
    {
        _infoOn = false;
        SitPlayerDown();
    }

    public void SitPlayerDown() 
    {
        Debug.Log("Player sits down");
        // Change position of player to sit down
        // Change UI to add cards
        // Add option to leave card game
    }

    public void ShowInfo()
    {
        if (!_infoOn) { 
            infoField.SetActive(true);
            Invoke("HideInfo", 3);
            textGameObject.transform.position = _initialPos;
            _infoOn = true;
        }
    }
    public void HideInfo()
    {
        _infoOn = false;
        infoField.SetActive(false);
    }

    public bool GetInfo()
    {
        return _infoOn;
    }

}
