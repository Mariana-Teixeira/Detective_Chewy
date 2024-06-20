using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class coinScript : MonoBehaviour
{
    [SerializeField] GameObject Discard;
    [SerializeField] GameObject Buy;
    [SerializeField] GameObject Sell;


    [SerializeField] GameObject Table1;
    [SerializeField] GameObject Table2;
    [SerializeField] GameObject Table3;

    [SerializeField] Animator anim;


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
            Buy.SetActive(false);
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

    public void MoveToTable2() {

        this.transform.parent.SetParent(Table2.gameObject.transform);
        this.transform.parent.localPosition = new Vector3(-0.224999994f, -0.0577000268f, 0.0250000656f);

    }
    public void MoveToTable3()
    {

        this.transform.parent.SetParent(Table3.gameObject.transform);
        this.transform.parent.localPosition = new Vector3(-0.224999994f, -0.0577000268f, 0.0250000656f);

    }


}
