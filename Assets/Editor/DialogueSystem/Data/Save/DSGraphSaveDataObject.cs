using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "DSGraphSaveDataObject", menuName = "Scriptable Objects/DSGraphSaveDataObject")]
public class DSGraphSaveDataObject : ScriptableObject
{
    public string FileName;
    public List<DSGroupSaveData> Groups;
    public List<DSNodeSaveData> Nodes;
    public List<string> OldGroupNames;
    public List<string> OldUngroupedNodeNames;
    public SerializableDictionary<string, List<string>> OldGroupedNodeNames;

    public void Init(string fileName)
    {
        FileName = fileName;

        Groups = new();
        Nodes = new();
    }

}
