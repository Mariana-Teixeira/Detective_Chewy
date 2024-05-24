using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class coinScript : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
      anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void FlipTheCoin(string s) 
    {
        if (s == "discard")
        {
            //discard mesh of the coin
        }
        else if (s == "buy") {
            //buy mesh of the coin
        }
        else {
            //sell mesh of the coin
        }
        anim.SetTrigger("flip_coin");
    }
}
