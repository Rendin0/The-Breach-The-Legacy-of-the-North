using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSMultipleChoiceNode : DSNode
{
    public override void Init(Vector2 position)
    {
        base.Init(position);
        DialogueType = DSDialogueType.MultipleChoice;

        Choices.Add("New choice");
    }

    public override void Draw()
    {
        base.Draw();

        Button addChoiceButton = DSElementUtility.CreateButton("Add choice", () =>
        {
            Port choicePort = CreatePort("New choice");
            Choices.Add("New choice");
            outputContainer.Add(choicePort);
        });

        addChoiceButton.AddToClassList("ds-node__button");

        mainContainer.Insert(1, addChoiceButton);

        foreach (var choice in Choices)
        {
            Port choicePort = CreatePort(choice);

            outputContainer.Add(choicePort);
        }

        RefreshExpandedState();
    }

    private Port CreatePort(string choice)
    {
        Port choicePort = this.CreatePort();

        choicePort.portName = "";

        Button deleteChoiceButton = DSElementUtility.CreateButton("X");
        deleteChoiceButton.AddToClassList("ds-node__button");

        TextField choiceTextField = DSElementUtility.CreateTextField(choice);

        choiceTextField.AddClasses(
            "ds-node__text-field",
            "ds-node__choice-text-field",
            "ds-node__text-field__hidden"
            );

        choicePort.Add(choiceTextField);
        choicePort.Add(deleteChoiceButton);
        return choicePort;
    }
}
