using TMPro;
using UnityEngine;

public class PopupElementInfoBinder : PopupBinder<PopupElementInfoViewModel>
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private RectTransform _container;
    private readonly Vector3 _offset = new(0, 20);

    protected override void OnBind(PopupElementInfoViewModel viewModel)
    {
        base.OnBind(viewModel);

        _text.text = viewModel.Text;
        _container.position = Input.mousePosition + new Vector3(0, 6000);
    }

    private void Update()
    {
        _container.position = Input.mousePosition + _offset;
    }
}