using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Collect Things")]
public class CollectThingsQuest : Quest
{
    public Thing[] Things;
}

[Serializable]
public struct Thing
{
    public string ThingName;
    public DialogueBranch Dialogue;

    public override string ToString()
    {
        return ThingName;
    }
}