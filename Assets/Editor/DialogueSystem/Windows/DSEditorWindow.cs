using System;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DSEditorWindow : EditorWindow
{
    private DSGraphView _graphView;
    private readonly string _defaultFileName = "Dialogues";
    private Button _saveButton;
    private static TextField _filename;


    [MenuItem("Window/Dialogue System/Dialogue graph")]
    public static void ShowExample()
    {
        GetWindow<DSEditorWindow>("Dialogue Graph");
    }

    private void CreateGUI()
    {
        AddGraphView();
        AddToolbar();

        AddStyles();
    }

    private void AddToolbar()
    {
        Toolbar toolbar = new();

        _filename = DSElementUtility.CreateTextField(_defaultFileName, "File Name: ", callback =>
        {
            _filename.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
        });
        _saveButton = DSElementUtility.CreateButton("Save", () => Save());

        Button clearButton = DSElementUtility.CreateButton("Clear", () => Clear());
        Button resetButton = DSElementUtility.CreateButton("Reset", () => ResetGraph());
        Button loadButton = DSElementUtility.CreateButton("Load", () => Load());
        ToolbarSpacer toolbarSpacer = new();
        ToolbarSpacer toolbarSpacer1 = new();
        ToolbarSpacer toolbarSpacer2 = new();
        

        toolbar.Add(_filename);
        toolbar.Add(_saveButton);
        toolbar.Add(loadButton);


        toolbar.Add(toolbarSpacer);
        toolbar.Add(toolbarSpacer1);
        toolbar.Add(toolbarSpacer2);
        toolbar.Add(clearButton);
        toolbar.Add(resetButton);

        toolbar.AddStyleSheets("DialogueSystem/DSToolbarStyles.uss");

        rootVisualElement.Add(toolbar);
    }

    private void Load()
    {
        string path = EditorUtility.OpenFilePanel("Dialogue graph", "Assets/Editor/DialogueSystem/Graphs", "asset");

        if (string.IsNullOrEmpty(path))
            return;

        Clear();
        DSIOUtility.Init(_graphView, Path.GetFileNameWithoutExtension(path));
        DSIOUtility.Load();
    }

    public static void UpdateFileName(string newFileName)
    {
        _filename.value = newFileName;
    }

    private void ResetGraph()
    {
        Clear();
        UpdateFileName(_defaultFileName);
    }

    private void Clear()
    {
        _graphView.ClearGraph();
    }

    private void Save()
    {
        if (string.IsNullOrEmpty(_filename.value))
        {
            EditorUtility.DisplayDialog(
                "Invalid file name",
                "Change file name",
                "ok)"
                );
            return;
        }

        DSIOUtility.Init(_graphView, _filename.value);
        DSIOUtility.Save();
    }

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets(
            "DialogueSystem/DSVariables.uss"
            );
    }

    private void AddGraphView()
    {
        _graphView = new DSGraphView(this);
        _graphView.StretchToParentSize();

        rootVisualElement.Add(_graphView);
    }

    public void EnableSaveButton()
    {
        _saveButton.SetEnabled(true);
    }

    public void DisableSaveButton()
    {
        _saveButton.SetEnabled(false);
    }
}
