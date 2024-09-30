using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Quest Order")]
public class QuestOrder : ScriptableObject
{
    public Quest[] Quests;
}