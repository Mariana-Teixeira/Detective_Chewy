using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class coinScript : MonoBehaviour
{
    Animator anim;
    [SerializeField] GameObject Discard;
    [SerializeField] GameObject Buy;
    [SerializeField] GameObject Sell;
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
        StartCoroutine(ChangeCoinType(s));
        anim.SetTrigger("flip_coin");
    }

    private IEnumerator ChangeCoinType(string s) {

        yield return new WaitForSeconds(0.3f);

        if (s == "discard")
        {
            //discard mesh of the coin
            Discard.SetActive(true);
            Sell.SetActive(false);
        }
        else if (s == "buy")
        {
            //buy mesh of the coin
            Discard.SetActive(false);
            Buy.SetActive(true);
        }
        else
        {
            //sell mesh of the coin
            Buy.SetActive(false);
            Sell.SetActive(true);
        }
    }
}
