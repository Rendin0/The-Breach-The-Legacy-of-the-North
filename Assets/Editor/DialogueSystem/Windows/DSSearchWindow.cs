using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DSSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private DSGraphView _graphView;
    private Texture2D _indentaionIcon;
    public void Init(DSGraphView graphView)
    {
        _graphView = graphView;

        _indentaionIcon = new(1, 1);
        _indentaionIcon.SetPixel(0, 0, Color.clear);
        _indentaionIcon.Apply();
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var searchTreeEntries = new List<SearchTreeEntry>()
        {
            new SearchTreeGroupEntry(new GUIContent("Create Element")),
            new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
            new SearchTreeEntry(new GUIContent("Single Choice", _indentaionIcon))
            {
                level = 2,
                userData = DSDialogueType.SingleChoice
            },
            new SearchTreeEntry(new GUIContent("Single Choice", _indentaionIcon))
            {
                level = 2,
                userData = DSDialogueType.MultipleChoice
            },
            new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
            new SearchTreeEntry(new GUIContent("Single Group", _indentaionIcon))
            {
                level = 2,
                userData = new Group()
            }
        };
        return searchTreeEntries;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        Vector2 localMousePos = _graphView.GetLocalMousePosition(context.screenMousePosition, true);

        switch (SearchTreeEntry.userData)
        {
            case DSDialogueType.SingleChoice:
                {
                    DSSingleChoiceNode singleChoiceNode = (DSSingleChoiceNode)_graphView.CreateNode(DSDialogueType.SingleChoice, localMousePos);
                    _graphView.AddElement(singleChoiceNode);

                    return true;
                }

            case DSDialogueType.MultipleChoice:
                {
                    DSMultipleChoiceNode multipleChoiceNode = (DSMultipleChoiceNode)_graphView.CreateNode(DSDialogueType.MultipleChoice, localMousePos);
                    _graphView.AddElement(multipleChoiceNode);

                    return true;
                }

            case Group _:
                {
                    Group group = _graphView.CreateGroup("Dialogue group", localMousePos);
                    _graphView.AddElement(group);

                    return true;
                }

            default:
                return false;

        }

    }
}
