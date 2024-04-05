using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCharacterController : MonoBehaviour
{
    #region Audio Messages
    //Rplace strings with audios
    private string[] audioQWalkIn = {
                "Proving P = NP...",
                "Feeding developers...",
                "Crying in bed..."

    };

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.tag == "Player") 
        {
            int randAQIndex = Random.Range(0, audioQWalkIn.Length);
            Debug.Log(audioQWalkIn[randAQIndex]);
        }
    }
}
