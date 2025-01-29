using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSGraphView : GraphView
{
    private DSSearchWindow _searchWindow;
    private DSEditorWindow _editorWindow;

    public DSGraphView(DSEditorWindow dsEditorWindow)
    {
        AddManipulators();
        //AddSearchWindow();
        AddGridBackground();

        AddStyles();
        _editorWindow = dsEditorWindow;
    }

    private void AddSearchWindow()
    {
        if (_searchWindow == null)
        {
            _searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();

            _searchWindow.Init(this);
        }

        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    public DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
    {
        Type nodeType = Type.GetType($"DS{dialogueType}Node");

        DSNode node = (DSNode)Activator.CreateInstance(nodeType);
        node.Init(position);
        node.Draw();

        return node;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new();

        ports.ForEach(port =>
        {
            if (startPort.node == port.node)
                return;

            if (startPort.direction == port.direction)
                return;

            compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }

    private void AddManipulators()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        this.AddManipulator(CreateNodeContextualMenu(DSDialogueType.SingleChoice, "Add Node (Single)"));
        this.AddManipulator(CreateNodeContextualMenu(DSDialogueType.MultipleChoice, "Add Node (Multiple)"));
        this.AddManipulator(CreateGroupContextualMenu());
    }

    private IManipulator CreateGroupContextualMenu()
    {
        ContextualMenuManipulator manipulator = new(
            menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );

        return manipulator;
    }

    public Group CreateGroup(string title, Vector2 localMousePosition)
    {
        Group group = new()
        {
            title = title
        };
        group.SetPosition(new Rect(localMousePosition, Vector2.zero));
        return group;
    }

    private IManipulator CreateNodeContextualMenu(DSDialogueType dialogueType, string actionTitle)
    {
        ContextualMenuManipulator manipulator = new(
            menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );

        return manipulator;
    }

    private void AddStyles()
    {
        this.AddStyleSheets(
            "DialogueSystem/DSGraphViewStyles.uss",
            "DialogueSystem/DSNodeStyles.uss"
            );
    }

    private void AddGridBackground()
    {
        GridBackground grid = new();
        grid.StretchToParentSize();

        Insert(0, grid);
    }

    public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
    {
        Vector2 worldMousePos = mousePosition;
        
        if (isSearchWindow)
        {
            worldMousePos -= _editorWindow.position.position;
        }

        Vector2 localMousePos = contentViewContainer.WorldToLocal(mousePosition);

        return localMousePos;
    }
}
