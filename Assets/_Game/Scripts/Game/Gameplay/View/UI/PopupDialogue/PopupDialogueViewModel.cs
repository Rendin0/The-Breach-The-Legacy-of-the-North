
public class PopupDialogueViewModel : WindowViewModel
{
    public override string Id => "PopupDialogue";

    private DialogueObject _currDialogue;

    public PopupDialogueViewModel(DialogueObject currDialogue)
    {
        _currDialogue = currDialogue;
    }
}
