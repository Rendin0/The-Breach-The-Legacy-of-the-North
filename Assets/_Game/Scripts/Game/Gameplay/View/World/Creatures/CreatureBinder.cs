
using UnityEngine;
using R3;

public class CreatureBinder : MonoBehaviour
{
    protected virtual void OnBind(CreatureViewModel viewModel) { }

    private CreatureViewModel _viewModel;

    private void Update()
    {
        _viewModel.Position.OnNext(transform.position);
    }

    public virtual void Bind(CreatureViewModel viewModel)
    {
        _viewModel = viewModel;
        OnBind(viewModel);
    }
}