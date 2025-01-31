using UnityEngine;

//[CreateAssetMenu(fileName = "DialogueGroupConfig", menuName = "Scriptable Objects/DialogueGroupConfig")]
public class DialogueGroupConfig : ScriptableObject
{
    public string GroupName;

    public void Init(string groupName)
    {
        GroupName = groupName;
    }
}
