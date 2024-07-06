using System.Collections;
using UnityEngine;


public class CoinScript : MonoBehaviour
{
    [SerializeField] GameObject Discard;
    [SerializeField] GameObject Buy;
    [SerializeField] GameObject Sell;

    [SerializeField] Board _board;
    [SerializeField] GameObject[] _tables;
    [SerializeField] Animator _animator;

    private void Awake()
    {
        _tables = new GameObject[_board.Tables.Length];
        for (int i = 0; i < _tables.Length; i++)
        {
            _tables[i] = _board.Tables[i]._tableObject;
        }
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void FlipTheCoin(string s) 
    {
        StartCoroutine(ChangeCoinType(s));
        _animator.SetTrigger("flip_coin");
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

    public void MoveToTable(int number)
    {
        this.transform.SetParent(_tables[number].gameObject.transform, false);
        this.transform.rotation = _tables[number].transform.rotation;
    }
}
