
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_", menuName = "Dialogues")]
public class DialogueObject : ScriptableObject
{
    [SerializeField][TextArea] private string[] _lines;


}

