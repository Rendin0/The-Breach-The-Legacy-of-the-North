using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DSMultipleChoiceNode : DSNode
{
    public override void Init(string nodeName, DSGraphView graphView, Vector2 position)
    {
        base.Init(nodeName, graphView, position);
        DialogueType = DSDialogueType.MultipleChoice;
        DSChoiceSaveData choice = new()
        {
            Text = "New Choice"
        };

        Choices.Add(choice);
    }

    public override void Draw()
    {
        base.Draw();

        Button addChoiceButton = DSElementUtility.CreateButton("Add choice", () =>
        {
            DSChoiceSaveData choiceData = new()
            {
                Text = "New Choice"
            };

            Choices.Add(choiceData);

            Port choicePort = CreatePort(choiceData);
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

    private Port CreatePort(object userData)
    {
        Port choicePort = this.CreatePort();

        choicePort.userData = userData;
        choicePort.portName = "";
        DSChoiceSaveData choiceData = (DSChoiceSaveData)userData;

        Button deleteChoiceButton = DSElementUtility.CreateButton("X", () =>
        {
            if (Choices.Count == 1)
                return;

            if (choicePort.connected)
            {
                graphView.DeleteElements(choicePort.connections);
            }

            Choices.Remove(choiceData);
            graphView.RemoveElement(choicePort);
        });
        deleteChoiceButton.AddToClassList("ds-node__button");

        TextField choiceTextField = DSElementUtility.CreateTextField(choiceData.Text, onValueChanged: callback =>
        {
            choiceData.Text = callback.newValue;
        });

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
