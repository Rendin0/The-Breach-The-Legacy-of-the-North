using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "DialogueConfig", menuName = "Scriptable Objects/DialogueConfig")]
public class DialogueConfig : ScriptableObject
{
    public string DialogueName;
    [TextArea()] public string Text;
    public List<DialogueChoiceData> Choices;
    public DSDialogueType DialogueType;
    public bool IsStartingDialogue;

    public void Init(string dialogueName, string text, List<DialogueChoiceData> choices, DSDialogueType dialogueType, bool isStartingDialogue)
    {
        DialogueName = dialogueName;
        Text = text;
        Choices = choices;
        DialogueType = dialogueType;
        IsStartingDialogue = isStartingDialogue;
    }
}
