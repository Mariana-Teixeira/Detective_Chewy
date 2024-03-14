using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class Card : MonoBehaviour
{
    private CardData _cardData;
    private TMP_Text _cardText;
    public CardData CardData
    {
        get
        {
            return _cardData;
        }
        set
        {
            _cardData = value;
        }
    }
    private void Awake()
    {
        _cardText = GetComponentInChildren<TMP_Text>();
    }

    public void UpdateUI()
    {
        _cardText.text = _cardData.ToString();
    }

    
}
