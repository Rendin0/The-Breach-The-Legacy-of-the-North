using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class CreatureBinder : MonoBehaviour, IPointerClickHandler
{
    protected Rigidbody2D rb;

    public abstract CreatureViewModel ViewModel { get; }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ViewModel.Rb = rb;
    }

    protected virtual void FixedUpdate()
    {
        ViewModel.Position.OnNext(rb.position);
    }

    public void Bind(CreatureViewModel viewModel)
    {
        OnBind(viewModel);

        ViewModel.Transform = transform;
        transform.position = ViewModel.Position.Value;
    }
    protected abstract void OnBind(CreatureViewModel viewModel);
    public void OnPointerClick(PointerEventData eventData)
    {
        ViewModel.OnClick(eventData);
    }
}