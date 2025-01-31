using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using DSNodeGroup = SerializableDictionary<string, DSNodeErrorData>;

public class DSGraphView : GraphView
{
    private DSSearchWindow _searchWindow;
    private DSEditorWindow _editorWindow;
    private DSNodeGroup _ungroupedNodes = new();
    private SerializableDictionary<Group, DSNodeGroup> _groupedNodes = new();
    private SerializableDictionary<string, DSGroupErrorData> _groups = new();
    private int _errorsAmount;
    public int ErrorsAmount
    {
        get
        {
            return _errorsAmount;
        }
        set
        {
            _errorsAmount = value;
            if (_errorsAmount == 0)
                _editorWindow.EnableSaveButton();
            else
                _editorWindow.DisableSaveButton();
        }
    }

    public DSGraphView(DSEditorWindow dsEditorWindow)
    {
        AddManipulators();
        //AddSearchWindow();
        AddGridBackground();

        OnElementsDeleted();
        OnGroupElementsAdded();
        OnGroupElementsRemoved();
        OnGroupRenamed();
        OnGraphViewChanged();

        AddStyles();
        _editorWindow = dsEditorWindow;
    }



    public DSGroup CreateGroup(string title, Vector2 localMousePosition)
    {
        DSGroup group = new(title, localMousePosition);

        AddGroup(group);
        AddElement(group);


        foreach (var element in selection)
        {
            if (element is not DSNode)
                continue;

            DSNode node = (DSNode)element;
            group.AddElement(node);
        }

        return group;
    }
    private void AddGroup(DSGroup group)
    {
        string groupName = group.title.ToLower();
        if (!_groups.ContainsKey(groupName))
        {
            DSGroupErrorData errorData = new();
            errorData.Groups.Add(group);
            _groups.Add(groupName, errorData);
            return;
        }

        List<DSGroup> groupList = _groups[groupName].Groups;

        groupList.Add(group);
        Color color = _groups[groupName].ErrorData.Color;
        group.SetErrorStyle(color);

        if (groupList.Count == 2)
        {
            ++ErrorsAmount;
            groupList[0].SetErrorStyle(color);
        }
    }
    private void RemoveGroup(DSGroup group)
    {
        string groupName = group.OldTitle.ToLower();

        List<DSGroup> groupList = _groups[groupName].Groups;

        groupList.Remove(group);
        group.ResetStyles();

        if (groupList.Count == 1)
        {
            --ErrorsAmount;
            groupList[0].ResetStyles();
            return;
        }

        if (groupList.Count == 0)
        {
            _groups.Remove(groupName);
        }
    }

    private void OnGroupRenamed()
    {
        groupTitleChanged = (group, newTitle) =>
        {
            DSGroup dsGroup = (DSGroup)group;

            dsGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

            if (string.IsNullOrEmpty(dsGroup.title))
            {
                if (!string.IsNullOrEmpty(dsGroup.OldTitle))
                    ++ErrorsAmount;
            }
            else
            {
                if (string.IsNullOrEmpty(dsGroup.OldTitle))
                    --ErrorsAmount;
            }

            RemoveGroup(dsGroup);
            dsGroup.OldTitle = dsGroup.title;
            AddGroup(dsGroup);
        };
    }
    private void OnGroupElementsAdded()
    {
        elementsAddedToGroup = (group, elements) =>
        {
            foreach (var element in elements)
            {
                if (element is not DSNode)
                {
                    continue;
                }

                DSNode node = (DSNode)element;
                DSGroup dsGroup = (DSGroup)group;

                RemoveUngroupedNode(node);
                AddGroupedNode(node, dsGroup);

            }
        };
    }
    private void OnGroupElementsRemoved()
    {
        elementsRemovedFromGroup = (group, elements) =>
        {
            foreach (var element in elements)
            {
                if (element is not DSNode)
                {
                    continue;
                }

                DSNode node = (DSNode)element;

                RemoveGroupedNode(node, group);
                AddUngroupedNode(node);
            }
        };
    }
    private void OnElementsDeleted()
    {
        deleteSelection = (_, _) =>
        {
            List<Edge> edgesToDelete = new();
            List<DSGroup> groupsToDelete = new();
            List<DSNode> nodesToDelete = new();

            foreach (var element in selection)
            {
                if (element is DSNode node)
                {
                    nodesToDelete.Add(node);
                    continue;
                }

                if (element is Edge edge)
                {
                    edgesToDelete.Add(edge);
                    continue;
                }

                if (element is DSGroup group)
                {
                    groupsToDelete.Add(group);
                    continue;
                }
            }

            foreach (var group in groupsToDelete)
            {
                List<DSNode> nodeList = new();

                foreach (var element in group.containedElements)
                {
                    if (element is not DSNode)
                        continue;

                    DSNode node = (DSNode)element;
                    nodeList.Add(node);
                }
                group.RemoveElements(nodeList);
                RemoveGroup(group);
                RemoveElement(group);
            }

            DeleteElements(edgesToDelete);

            foreach (var node in nodesToDelete)
            {
                node.Group?.RemoveElement(node);
                RemoveUngroupedNode(node);
                node.DisconnectAllPorts();
                RemoveElement(node);
            }
        };
    }
    private void OnGraphViewChanged()
    {
        graphViewChanged = changes =>
        {
            if (changes.edgesToCreate != null)
            {
                foreach (var edge in changes.edgesToCreate)
                {
                    DSNode nextNode = (DSNode)edge.input.node;

                    DSChoiceSaveData choiceData = (DSChoiceSaveData)edge.output.userData;
                    choiceData.NodeId = nextNode.Id;
                }
            }

            if (changes.elementsToRemove != null)
            {
                foreach (var element in changes.elementsToRemove)
                {
                    if (element is Edge edge)
                    {
                        DSChoiceSaveData choiceData = (DSChoiceSaveData)edge.output.userData;
                        choiceData.NodeId = "";
                    }
                }
            }

            return changes;
        };
    }

    public DSNode CreateNode(string nodeName, DSDialogueType dialogueType, Vector2 position, bool shouldDraw = true)
    {
        Type nodeType = Type.GetType($"DS{dialogueType}Node");

        DSNode node = (DSNode)Activator.CreateInstance(nodeType);
        node.Init(nodeName, this, position);

        if (shouldDraw)
            node.Draw();

        AddUngroupedNode(node);

        return node;
    }
    public void RemoveGroupedNode(DSNode node, Group group)
    {
        string nodeName = node.DialogueName.ToLower();

        node.Group = null;

        List<DSNode> nodeList = _groupedNodes[group][nodeName].Nodes;

        nodeList.Remove(node);
        node.ResetStyle();

        if (nodeList.Count == 1)
        {
            --ErrorsAmount;
            nodeList[0].ResetStyle();
            return;
        }

        if (nodeList.Count == 0)
        {
            _groupedNodes[group].Remove(nodeName);
            if (_groupedNodes[group].Count == 0)
            {
                _groupedNodes.Remove(group);
            }
        }
    }
    public void AddGroupedNode(DSNode node, DSGroup group)
    {
        string nodeName = node.DialogueName.ToLower();

        node.Group = group;

        if (!_groupedNodes.ContainsKey(group))
        {
            _groupedNodes.Add(group, new DSNodeGroup());
        }

        if (!_groupedNodes[group].ContainsKey(nodeName))
        {
            DSNodeErrorData errorData = new();
            errorData.Nodes.Add(node);

            _groupedNodes[group].Add(nodeName, errorData);
            return;
        }

        List<DSNode> nodeList = _groupedNodes[group][nodeName].Nodes;

        nodeList.Add(node);
        Color errorColor = _groupedNodes[group][nodeName].ErrorData.Color;

        node.SetErrorStyle(errorColor);


        if (nodeList.Count == 2)
        {
            ++ErrorsAmount;
            nodeList[0].SetErrorStyle(errorColor);
        }
    }
    public void AddUngroupedNode(DSNode node)
    {
        string nodeName = node.DialogueName.ToLower();
        if (!_ungroupedNodes.ContainsKey(nodeName))
        {
            DSNodeErrorData nodeError = new();
            nodeError.Nodes.Add(node);
            _ungroupedNodes.Add(nodeName, nodeError);
            return;
        }

        _ungroupedNodes[nodeName].Nodes.Add(node);
        Color errorColor = _ungroupedNodes[nodeName].ErrorData.Color;
        node.SetErrorStyle(errorColor);

        if (_ungroupedNodes[nodeName].Nodes.Count == 2)
        {
            ++ErrorsAmount;
            _ungroupedNodes[nodeName].Nodes[0].SetErrorStyle(errorColor);
        }
    }
    public void RemoveUngroupedNode(DSNode node)
    {
        string nodeName = node.DialogueName.ToLower();


        _ungroupedNodes[nodeName].Nodes.Remove(node);
        node.ResetStyle();

        if (_ungroupedNodes[nodeName].Nodes.Count == 1)
        {
            --ErrorsAmount;
            _ungroupedNodes[nodeName].Nodes[0].ResetStyle();
        }

        if (_ungroupedNodes[nodeName].Nodes.Count == 0)
        {
            _ungroupedNodes.Remove(nodeName);
        }

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
            menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
            );

        return manipulator;
    }
    private IManipulator CreateNodeContextualMenu(DSDialogueType dialogueType, string actionTitle)
    {
        ContextualMenuManipulator manipulator = new(
            menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode("DialogueName", dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
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
    private void AddSearchWindow()
    {
        if (_searchWindow == null)
        {
            _searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();

            _searchWindow.Init(this);
        }

        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    public void ClearGraph()
    {
        DeleteElements(graphElements);

        _groups.Clear();
        _groupedNodes.Clear();
        _ungroupedNodes.Clear();

        ErrorsAmount = 0;
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
