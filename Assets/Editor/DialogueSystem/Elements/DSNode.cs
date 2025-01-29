using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSNode : Node
{
    public string DialogueName { get; set; }
    public List<string> Choices { get; set; }
    public string Text { get; set; }
    public DSDialogueType DialogueType { get; set; }

    public virtual void Init(Vector2 position)
    {
        DialogueName = "DialogueName";
        Choices = new();
        Text = "Text.";

        SetPosition(new Rect(position, Vector2.zero));

        mainContainer.AddToClassList("ds-node__main-container");
        extensionContainer.AddToClassList("ds-node__extension-container");
    }

    public virtual void Draw()
    {
        // Title

        TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName);

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
        TextField textTextField = DSElementUtility.CreateTextArea(Text);

        textTextField.AddClasses(
            "ds-node__text-field",
            "ds-node__quote-text-field"
            );

        textFoldout.Add(textTextField);

        customDataContainer.Add(textFoldout);

        extensionContainer.Add(customDataContainer);

    }
}
