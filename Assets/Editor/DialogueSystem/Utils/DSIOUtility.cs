using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class DSIOUtility
{
    private static DSGraphView _graphView;
    private static string _graphFileName;
    private static string _configFolderPath;

    private static List<DSGroup> _groups;
    private static List<DSNode> _nodes;

    private static Dictionary<string, DialogueGroupConfig> _createdDialogueGroups;
    private static Dictionary<string, DialogueConfig> _createdDialogues;
    private static Dictionary<string, DSGroup> _loadedGroups;
    private static Dictionary<string, DSNode> _loadedNodes;
    public static void Init(DSGraphView graphView, string graphName)
    {
        _graphView = graphView;
        _graphFileName = graphName;
        _configFolderPath = $"Assets/_Game/Configs/Dialogues/{_graphFileName}";

        _groups = new();
        _nodes = new();
        _createdDialogueGroups = new();
        _createdDialogues = new();
        _loadedGroups = new();
        _loadedNodes = new();
    }

    public static void Save()
    {
        CreateStaticFolders();

        GetElemetsFromGraphView();

        DSGraphSaveDataObject graphData = CreateAsset<DSGraphSaveDataObject>("Assets/Editor/DialogueSystem/Graphs", $"{_graphFileName}Graph");
        graphData.Init(_graphFileName);

        DialogueContainerConfig dialogueContainer = CreateAsset<DialogueContainerConfig>(_configFolderPath, _graphFileName);
        dialogueContainer.Init(_graphFileName);

        SaveGroups(graphData, dialogueContainer);
        SaveNodes(graphData, dialogueContainer);

        SaveAsset(graphData);
        SaveAsset(dialogueContainer);

    }
    public static void Load()
    {
        DSGraphSaveDataObject graphData = LoadAsset<DSGraphSaveDataObject>("Assets/Editor/DialogueSystem/Graphs", _graphFileName);

        if (graphData == null )
        {
            EditorUtility.DisplayDialog(
                "Couldn't load file",
                "The file at path could not be found: \n\n" +
                $"Assets/Editor/DialogueSystem/Graphs/{_graphFileName}",
                "ok"
                );

            return;
        }

        DSEditorWindow.UpdateFileName(graphData.FileName);
        LoadGroups(graphData.Groups);
        LoadNodes(graphData.Nodes);
        LoadNodesConnections();
    }

    private static void LoadNodesConnections()
    {
        foreach (var node in _loadedNodes)
        {
            foreach (Port choicePort in node.Value.outputContainer.Children())
            {
                DSChoiceSaveData choiceData = (DSChoiceSaveData) choicePort.userData;

                if (string.IsNullOrEmpty(choiceData.NodeId))
                    continue;

                DSNode nextNode = _loadedNodes[choiceData.NodeId];
                Port nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();
                Edge edge = choicePort.ConnectTo(nextNodeInputPort);
                _graphView.AddElement(edge);
                node.Value.RefreshPorts();
            }
        }
    }

    private static void LoadNodes(List<DSNodeSaveData> nodes)
    {
        foreach (var nodeData in nodes)
        {
            var choices = CloneNodeChoices(nodeData.Choices);

            DSNode node = _graphView.CreateNode(nodeData.Name, nodeData.DialogueType, nodeData.Position, false);
            node.Id = nodeData.Id;
            node.Choices = choices;
            node.Text = nodeData.Text;
            node.Draw();

            _graphView.AddElement(node);
            _loadedNodes.Add(node.Id, node);

            if (string.IsNullOrEmpty(nodeData.GroupId))
                continue;

            DSGroup group = _loadedGroups[nodeData.GroupId];
            node.Group = group;
            group.AddElement(node);

        }
    }

    private static void LoadGroups(List<DSGroupSaveData> groups)
    {
        foreach (var groupData in groups)
        {
            DSGroup group = _graphView.CreateGroup(groupData.Name, groupData.Position);
            group.Id = groupData.Id;

            _loadedGroups.Add(group.Id, group);
        }
    }

    private static void SaveNodes(DSGraphSaveDataObject graphData, DialogueContainerConfig dialogueContainer)
    {
        SerializableDictionary<string, List<string>> groupedNodeNames = new();
        List<string> ungroupedNodeNames = new();

        foreach (var node in _nodes)
        {
            SaveNodeToGraph(node, graphData);
            SaveNodeToScriptableObject(node, dialogueContainer);

            if (node.Group != null)
                groupedNodeNames.AddItem(node.Group.title, node.DialogueName);
            else
                ungroupedNodeNames.Add(node.DialogueName);
        }

        UpdateDialoguesChoicesConnections();

        UpdateOldGroupedNodes(groupedNodeNames, graphData);

        UpdateUngroupedNodes(ungroupedNodeNames, graphData);
    }

    private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, DSGraphSaveDataObject graphData)
    {
        if (graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count != 0)
        {
            foreach (var oldGroupedNode in graphData.OldGroupedNodeNames)
            {
                List<string> nodesToRemove = new();

                if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                {
                    nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key]).ToList();
                }

                foreach (var nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{_configFolderPath}/Groups/{oldGroupedNode.Key}/Dialogues", nodeToRemove);
                }
            }
        }

        graphData.OldGroupedNodeNames = new(currentGroupedNodeNames);
    }

    private static void UpdateUngroupedNodes(List<string> currentUngroupedNodeNames, DSGraphSaveDataObject graphData)
    {
        if (graphData.OldNodeNames != null && graphData.OldNodeNames.Count != 0)
        {
            List<string> nodesToRemove = graphData.OldNodeNames.Except(currentUngroupedNodeNames).ToList();

            foreach (var nodeToRemove in nodesToRemove)
            {
                RemoveAsset($"{_configFolderPath}/Global/Dialogues", nodeToRemove);
            }
        }

        graphData.OldGroupNames = new(currentUngroupedNodeNames);

    }

    private static void RemoveAsset(string path, string assetName)
    {
        AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
    }

    private static void UpdateDialoguesChoicesConnections()
    {
        foreach (var node in _nodes)
        {
            DialogueConfig dialogue = _createdDialogues[node.Id];

            for (int i = 0; i < node.Choices.Count; i++)
            {
                DSChoiceSaveData nodeChoice = node.Choices[i];
                if (string.IsNullOrEmpty(nodeChoice.NodeId))
                    continue;

                dialogue.Choices[i].NextDialogue = _createdDialogues[nodeChoice.NodeId];

                SaveAsset(dialogue);
            }
        }
    }

    private static void SaveNodeToScriptableObject(DSNode node, DialogueContainerConfig dialogueContainer)
    {
        DialogueConfig dialogue;

        if (node.Group != null)
        {
            dialogue = CreateAsset<DialogueConfig>($"{_configFolderPath}/Groups/{node.Group.title}/Dialogues", node.DialogueName);

            dialogueContainer.DialogueGroups.AddItem(_createdDialogueGroups[node.Group.Id], dialogue);
        }
        else
        {
            dialogue = CreateAsset<DialogueConfig>($"{_configFolderPath}/Global/Dialogues", node.DialogueName);
            dialogueContainer.UngroupedDialogues.Add(dialogue);
        }

        dialogue.Init(
            node.DialogueName,
            node.Text,
            ConvertNodeChoicesToDialogueChoices(node.Choices),
            node.DialogueType,
            node.IsStartingNode()
            );

        _createdDialogues.Add(node.Id, dialogue);

        SaveAsset(dialogue);
    }

    private static List<DialogueChoiceData> ConvertNodeChoicesToDialogueChoices(List<DSChoiceSaveData> nodeChoices)
    {
        List<DialogueChoiceData> dialogueChoices = new();

        foreach (var nodeChoice in nodeChoices)
        {
            DialogueChoiceData choiceData = new()
            {
                Text = nodeChoice.Text
            };

            dialogueChoices.Add(choiceData);
        }

        return dialogueChoices;
    }

    private static void SaveNodeToGraph(DSNode node, DSGraphSaveDataObject graphData)
    {
        List<DSChoiceSaveData> choices = CloneNodeChoices(node.Choices);

        DSNodeSaveData nodeData = new()
        {
            Id = node.Id,
            Name = node.DialogueName,
            Choices = choices,
            Text = node.Text,
            GroupId = node.Group?.Id,
            DialogueType = node.DialogueType,
            Position = node.GetPosition().position
        };

        graphData.Nodes.Add(nodeData);
    }

    private static List<DSChoiceSaveData> CloneNodeChoices(List<DSChoiceSaveData> nodeChoices)
    {
        List<DSChoiceSaveData> choices = new List<DSChoiceSaveData>();

        foreach (DSChoiceSaveData choice in nodeChoices)
        {
            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = choice.Text,
                NodeId = choice.NodeId
            };

            choices.Add(choiceData);
        }

        return choices;
    }

    private static void SaveGroups(DSGraphSaveDataObject graphData, DialogueContainerConfig dialogueContainer)
    {
        List<string> groupNames = new();

        foreach (var group in _groups)
        {
            SaveGroupToGraph(group, graphData);
            SaveGroupToScriptableObject(group, dialogueContainer);

            groupNames.Add(group.title);
        }

        UpdateOldGroups(groupNames, graphData);
    }

    private static void UpdateOldGroups(List<string> currentGroupNames, DSGraphSaveDataObject graphData)
    {
        if (graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
        {
            List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

            foreach (var groupToRemove in groupsToRemove)
            {
                RemoveFolder($"{_configFolderPath}/Groups/{groupsToRemove}");
            }
        }

        graphData.OldGroupNames = new(currentGroupNames);
    }

    private static void RemoveFolder(string fullPath)
    {
        FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
        FileUtil.DeleteFileOrDirectory($"{fullPath}/");
    }

    private static void SaveGroupToScriptableObject(DSGroup group, DialogueContainerConfig dialogueContainer)
    {
        string groupName = group.title;

        CreateFolder($"{_configFolderPath}/Groups", groupName);
        CreateFolder($"{_configFolderPath}/Groups/{groupName}", "Dialogues");

        DialogueGroupConfig dialogueGroup = CreateAsset<DialogueGroupConfig>($"{_configFolderPath}/Groups/{groupName}", groupName);
        dialogueGroup.Init(groupName);

        _createdDialogueGroups.Add(group.Id, dialogueGroup);

        dialogueContainer.DialogueGroups.Add(dialogueGroup, new());

        SaveAsset(dialogueGroup);
    }

    private static void SaveAsset(UnityEngine.Object asset)
    {
        EditorUtility.SetDirty(asset);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void SaveGroupToGraph(DSGroup group, DSGraphSaveDataObject graphData)
    {
        DSGroupSaveData data = new()
        {
            Id = group.Id,
            Name = group.title,
            Position = group.GetPosition().position
        };

        graphData.Groups.Add(data);
    }

    private static T CreateAsset<T>(string path, string fileName) where T : ScriptableObject
    {
        string fullPath = $"{path}/{fileName}.asset";
        T asset = LoadAsset<T>(path, fileName);

        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, fullPath);
        }

        return asset;
    }

    private static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        return AssetDatabase.LoadAssetAtPath<T>($"{path}/{assetName}.asset");
    }

    private static void GetElemetsFromGraphView()
    {
        _graphView.graphElements.ForEach(graphElement =>
        {
            if (graphElement is DSNode node)
            {
                _nodes.Add(node);
                return;
            }

            if (graphElement is DSGroup group)
            {
                _groups.Add(group);
                return;
            }
        });
    }

    private static void CreateStaticFolders()
    {
        CreateFolder("Assets/Editor/DialogueSystem", "Graphs");
        CreateFolder("Assets", "_Game");
        CreateFolder("Assets/_Game", "Configs");
        CreateFolder("Assets/_Game/Configs", "Dialogues");

        CreateFolder("Assets/_Game/Configs/Dialogues", _graphFileName);
        CreateFolder(_configFolderPath, "Global");
        CreateFolder($"{_configFolderPath}/Global", "Dialogues");

        CreateFolder(_configFolderPath, "Groups");

    }

    private static void CreateFolder(string path, string folderName)
    {
        if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
        {
            return;
        }

        AssetDatabase.CreateFolder(path, folderName);
    }
}
