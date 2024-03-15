using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public enum Phase
{
    Discard = 0,
    Buy = 1,
    Points = 2
}

public class Card : MonoBehaviour
{
    private CardData _cardData;
    private TMP_Text _cardText;
    private Outline _outline;

    private CardLogic _cardLogic;

    private bool _selected = false;

    public Phase Phase;


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
        _cardLogic = FindFirstObjectByType<CardLogic>();
        _outline = GetComponent<Outline>();
        _cardText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        CardLogic.OnChangePhase += ChangePhase;
    }

    public void ChangePhase(Phase phase) 
    {
        this.Phase = phase;
    }

    public void UpdateUI()
    {
        _cardText.text = _cardData.ToString();
    }

    void OnMouseOver()
    {
        _outline.enabled = true;
    }

    void OnMouseExit()
    {
        if (!_selected) {
            _outline.enabled = false;
        }
        else { 
        }
    }

    private void OnMouseDown()
    {
        if (!_selected) { 
            _selected = true;
            _outline.OutlineColor = Color.green;
            _cardLogic.SelectCard(this);
        }
        else { 
            _selected = false;
            _outline.OutlineColor = Color.yellow;
        }
        _outline.enabled = true;
    }

    public void UnselectCard() 
    {
        _selected = false;
        _outline.OutlineColor = Color.yellow;
        _outline.enabled = false;
    }


}
