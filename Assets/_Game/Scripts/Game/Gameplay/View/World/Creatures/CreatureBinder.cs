
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatureBinder : MonoBehaviour, IPointerClickHandler
{
    protected virtual void OnBind(CreatureViewModel viewModel) { }

    private CreatureViewModel _viewModel;

    private void Update()
    {
        _viewModel.Position.OnNext(transform.position);
    }

    public void Bind(CreatureViewModel viewModel)
    {
        _viewModel = viewModel;
        OnBind(viewModel);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _viewModel.OnClick(eventData);
    }
}