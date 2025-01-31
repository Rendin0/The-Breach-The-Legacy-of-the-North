using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DSNodeSaveData
{
    public string Id;
    public string Name;
    public string Text;
    public List<DSChoiceSaveData> Choices;
    public string GroupId;
    public DSDialogueType DialogueType;
    public Vector2 Position;
}
