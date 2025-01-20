
using UnityEngine;

public class EquipmentBinder : MonoBehaviour
{
    private PopupInventoryViewModel _viewModel;

    public void Bind(PopupInventoryViewModel viewModel)
    {
        _viewModel = viewModel;
    }
}