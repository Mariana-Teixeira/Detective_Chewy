using TMPro;
using UnityEngine;

public class InformationCanvasScript : MonoBehaviour
{
    public QuestManager QuestManager;
    public TMP_Text QuestText;

    private void Start()
    {
        ChangeText();
        QuestManager.CompleteQuest += ChangeText;
    }

    public void ChangeText()
    {
        var Q = QuestManager.CurrentQuest;
        QuestText.text = Q.QuestDescription;
    }
}