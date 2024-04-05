using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardGameAudioController : MonoBehaviour
{
    #region Audio Card Game Messages
    //replace strings with audios
    private static string[] firstGameAudioQ = new string[3] {
                "1.1test",
               "1.2test",
                "1.3test"

    };

    private static string[] secondGameAudioQ = new string[3]{
                "2.1test",
               "2.2test",
                "2.3test"

    };

    private static string[] ThirdGameAudioQ = new string[3]{
                "3.1test",
               "3.2test",
                "3.3test"

    };


    List<string[]> data = new List<string[]>() { firstGameAudioQ, secondGameAudioQ, ThirdGameAudioQ};

    #endregion

    public void ActivateAudioQ(int opponent, int AudioNum) 
    {
        Debug.Log(data.ElementAt(opponent)[AudioNum] + " ---------------- AUDIO!");
        //data.ElementAt(opponent)[AudioNum].play
    }

}
