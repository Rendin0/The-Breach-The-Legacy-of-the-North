
using UnityEngine;

public class DPInventoriesBinder : MonoBehaviour
{
    private DPInventoriesViewModel _viewModel;

    public void Bind(DPInventoriesViewModel viewModel)
    {
        _viewModel = viewModel;
    }

}