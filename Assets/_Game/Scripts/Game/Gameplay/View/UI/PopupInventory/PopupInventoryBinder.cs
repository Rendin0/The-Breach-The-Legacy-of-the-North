using UnityEngine;

public class PopupInventoryBinder : PopupBinder<PopupInventoryViewModel>
{
    [SerializeField] InventoryBinder _inventory;

    //[SerializeField] EquipmentBinder _equipment;

    protected override void Start()
    {
        _btnCloseAlt?.onClick.AddListener(OnExitButtonClicked);
        base.Start();

    }

    protected override void OnBind(PopupInventoryViewModel viewModel)
    {
        base.OnBind(viewModel);

        _inventory.Bind(viewModel, this);
        //_equipment.Bind(viewModel);
        ToggleEquipment();
    }


    private void OnExitButtonClicked()
    {
        ViewModel.RequestThrow();
    }

    public void ToggleEquipment()
    {
        //_equipment.gameObject.SetActive(!_equipment.gameObject.activeSelf);
    }
}