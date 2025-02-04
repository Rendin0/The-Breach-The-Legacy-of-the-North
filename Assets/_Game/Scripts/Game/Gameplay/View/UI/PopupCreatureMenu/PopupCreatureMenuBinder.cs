using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupCreatureMenuBinder : PopupBinder<PopupCreatureMenuViewModel>
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private TMP_Text _creatureNameText;
    [SerializeField] private Button _creatureInfoButton;
    [SerializeField] private Button _deleteButton;
    private PopupCreatureMenuViewModel _viewModel;

    private void Awake()
    {
        _creatureInfoButton.onClick.AddListener(() => OnCreatureInfoButtonClicked());
        _deleteButton.onClick.AddListener(() => OnDeleteButtonClicked());
    }

    private void OnDeleteButtonClicked()
    {
        _viewModel.DeleteCreature();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _creatureInfoButton.onClick.RemoveAllListeners();
        _deleteButton.onClick.RemoveAllListeners();
    }

    protected override void OnBind(PopupCreatureMenuViewModel viewModel)
    {
        base.OnBind(viewModel);
        _creatureNameText.text = $"{viewModel.CreatureName}\nId: {viewModel.CreatureId}";
        _container.position = viewModel.Position;

        _viewModel = viewModel;
    }

    private void OnCreatureInfoButtonClicked()
    {
        _viewModel.OpenCreatureInfo();
    }
}