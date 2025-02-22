using TMPro;
using UnityEngine;

public class PopupElementInfoBinder : PopupBinder<PopupElementInfoViewModel>
{
    [SerializeField] private TMP_Text _text;

    protected override void OnBind(PopupElementInfoViewModel viewModel)
    {
        base.OnBind(viewModel);

        _text.text = viewModel.Text;
    }
}