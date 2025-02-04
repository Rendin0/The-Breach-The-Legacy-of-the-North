using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSNode : Node
{
    public string DialogueName { get; set; }
    public List<DSChoiceSaveData> Choices { get; set; }
    public string Id { get; set; }
    public string Text { get; set; }
    public DSDialogueType DialogueType { get; set; }
    public DSGroup Group { get; set; }

    private Color _defaultBackgroundColor = new(29f / 255f, 29f / 255f, 30f / 255f);
    protected DSGraphView graphView;

    public virtual void Init(string nodeName, DSGraphView graphView, Vector2 position)
    {
        Id = Guid.NewGuid().ToString();

        DialogueName = nodeName;
        Choices = new();
        Text = "Text.";
        this.graphView = graphView;

        SetPosition(new Rect(position, Vector2.zero));

        mainContainer.AddToClassList("ds-node__main-container");
        extensionContainer.AddToClassList("ds-node__extension-container");
    }

    public virtual void Draw()
    {
        // Title

        TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, onValueChanged: callback =>
        {
            TextField target = (TextField)callback.target;
            target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            if (string.IsNullOrEmpty(target.value))
            {
                if (!string.IsNullOrEmpty(DialogueName))
                    ++graphView.ErrorsAmount;
            }
            else
            {
                if (string.IsNullOrEmpty(DialogueName))
                    --graphView.ErrorsAmount;
            }

            if (Group == null)
            {
                graphView.RemoveUngroupedNode(this);
                DialogueName = target.value;

                graphView.AddUngroupedNode(this);
                return;
            }

            var groupTmp = Group;

            graphView.RemoveGroupedNode(this, Group);
            DialogueName = target.value;
            graphView.AddGroupedNode(this, groupTmp);
        });

        dialogueNameTextField.AddClasses(
            "ds-node__text-field",
            "ds-node__filename-text-field",
            "ds-node__text-field__hidden"
            );

        titleContainer.Insert(0, dialogueNameTextField);

        // Input

        Port inputPort = this.CreatePort("Prev Dialogue", direction: Direction.Input, capacity: Port.Capacity.Multi);
        inputPort.name = "Prev dialogue";
        inputPort.portName = "Prev dialogue";

        inputContainer.Add(inputPort);

        // Extensions

        VisualElement customDataContainer = new();

        customDataContainer.AddToClassList("ds-node__custom-data-container");

        Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue text");
        TextField textTextField = DSElementUtility.CreateTextArea(Text, onValueChanged: callback =>
        {
            Text = callback.newValue;
        });

        textTextField.AddClasses(
            "ds-node__text-field",
            "ds-node__quote-text-field"
            );

        textFoldout.Add(textTextField);

        customDataContainer.Add(textFoldout);

        extensionContainer.Add(customDataContainer);

    }

    public void DisconnectAllPorts()
    {
        DisconnectPorts(inputContainer);
        DisconnectPorts(outputContainer);
    }
    private void DisconnectPorts(VisualElement container)
    {
        foreach (Port port in container.Children())
        {
            if (!port.connected)
                continue;

            graphView.DeleteElements(port.connections);
        }
    }

    public void SetErrorStyle(Color color)
    {
        mainContainer.style.backgroundColor = color;
    }

    public void ResetStyle()
    {
        mainContainer.style.backgroundColor = _defaultBackgroundColor;
    }

    public bool IsStartingNode()
    {
        Port inputPort = (Port)inputContainer.Children().First();

        return !inputPort.connected;
    }
}
