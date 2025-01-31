using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "DialogueContainerConfig", menuName = "Scriptable Objects/DialogueContainerConfig")]
public class DialogueContainerConfig : ScriptableObject
{
    public string Filename;
    public SerializableDictionary<DialogueGroupConfig, List<DialogueConfig>> DialogueGroups;
    public List<DialogueConfig> UngroupedDialogues;

    public void Init(string filename)
    {
        Filename = filename;

        DialogueGroups = new();
        UngroupedDialogues = new();
    }
}
