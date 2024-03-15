using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] GameObject player;

    private int _numOfInteractions = 0;
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other + " Interacted with NPC");
        _numOfInteractions++;
        AudioQ();
    }

    public void AudioQ() 
    {
        //Release one of the AudioQs
    }

}
