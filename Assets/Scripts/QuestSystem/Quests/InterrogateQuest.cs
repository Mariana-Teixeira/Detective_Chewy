using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Interrogate")]
public class InterrogateQuest : Quest
{
    public string Character;
    public string[] ThingNames;
    public DialogueBranch Branch;
}