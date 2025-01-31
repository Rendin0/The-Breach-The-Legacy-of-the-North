using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DSSingleChoiceNode : DSNode
{
    public override void Init(string nodeName, DSGraphView graphView, Vector2 position)
    {
        base.Init(nodeName, graphView, position);

        DialogueType = DSDialogueType.SingleChoice;

        DSChoiceSaveData choice = new()
        {
            Text = "Next Dialogue"
        };

        Choices.Add(choice);
    }

    public override void Draw()
    {
        base.Draw();

        foreach (var choice in Choices)
        {
            Port choicePort = this.CreatePort(choice.Text);

            choicePort.userData = choice;

            choicePort.portName = choice.Text;
            outputContainer.Add(choicePort);
        }

        RefreshExpandedState();
    }
}
