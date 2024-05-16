using System;
using TMPro;
using UnityEngine;

public class InformationCanvasScript : MonoBehaviour
{
    public static Action<bool> ToggleVisibility;
    public static Action<Quest> UpdateQuestInformation;

    private Canvas informationCanvas;
    public QuestManager QuestManager;
    public TMP_Text QuestText;

    private void Awake()
    {
        UpdateQuestInformation += OnUpdateQuestInformation;
        ToggleVisibility += OnToggleVisibility;
    }

    private void Start()
    {
        informationCanvas = GetComponent<Canvas>();
    }

    public void OnUpdateQuestInformation(Quest Q)
    {
        QuestText.text = Q.QuestDescription;
    }

    public void OnToggleVisibility(bool isVisible)
    {
        informationCanvas.enabled = isVisible;
    }
}