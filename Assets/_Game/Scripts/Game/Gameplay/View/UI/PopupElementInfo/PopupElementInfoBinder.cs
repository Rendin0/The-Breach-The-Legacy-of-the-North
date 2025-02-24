using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupElementInfoBinder : PopupBinder<PopupElementInfoViewModel>
{
    [SerializeField] private TMP_Text _textDescription;
    [SerializeField] private TMP_Text _textName;
    [SerializeField] private Image _imageIcon;

    [SerializeField] private RectTransform _container;
    private readonly Vector3 _offset = new(0, 20);

    protected override void OnBind(PopupElementInfoViewModel viewModel)
    {
        base.OnBind(viewModel);

        _textName.text = viewModel.ElementName;
        _textDescription.text = viewModel.Description;
        _imageIcon.sprite = viewModel.Icon;

        _container.position = Input.mousePosition + new Vector3(0, 6000);
    }

    private void Update()
    {
        _container.position = Input.mousePosition + _offset;
    }
}