using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DSSingleChoiceNode : DSNode
{
    public override void Init(Vector2 position)
    {
        base.Init(position);

        DialogueType = DSDialogueType.SingleChoice;

        Choices.Add("Next dialogue");
    }

    public override void Draw()
    {
        base.Draw();

        foreach (var choice in Choices)
        {
            Port choicePort = this.CreatePort(choice);

            choicePort.portName = choice;
            outputContainer.Add(choicePort);
        }

        RefreshExpandedState();
    }
}
