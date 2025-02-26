using R3;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class CreatureBinder : MonoBehaviour, IPointerClickHandler
{
    protected Rigidbody2D rb;
    protected virtual void OnBind(CreatureViewModel viewModel) { }
    protected bool movementBlocked = false;

    public CreatureViewModel ViewModel { get; private set; }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ViewModel.Rb = rb;
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        ViewModel.Position.OnNext(rb.position);
    }

    public void Bind(CreatureViewModel viewModel)
    {
        ViewModel = viewModel;
        transform.position = ViewModel.Position.Value;
        viewModel.MovementBlocked.Subscribe(b => movementBlocked = b);

        OnBind(viewModel);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ViewModel.OnClick(eventData);
    }
}