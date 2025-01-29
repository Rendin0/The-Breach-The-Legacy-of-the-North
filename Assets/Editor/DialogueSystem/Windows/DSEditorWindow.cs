using System;
using UnityEditor;
using UnityEngine.UIElements;

public class DSEditorWindow : EditorWindow
{
    [MenuItem("Window/Dialogue System/Dialogue graph")]
    public static void ShowExample()
    {
        GetWindow<DSEditorWindow>("Dialogue Graph");
    }

    private void CreateGUI()
    {
        AddGraphView();
        AddStyles();
    }

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets(
            "DialogueSystem/DSVariables.uss"
            );
    }

    private void AddGraphView()
    {
        DSGraphView graphView = new DSGraphView(this);
        graphView.StretchToParentSize();

        rootVisualElement.Add(graphView);
    }
}
