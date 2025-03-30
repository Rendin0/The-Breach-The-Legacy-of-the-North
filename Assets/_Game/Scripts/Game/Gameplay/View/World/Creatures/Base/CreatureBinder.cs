using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using R3;
using System;
using UnityEngine;
using UnityEngine.AI;
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

    public void Bind(CreatureViewModel viewModel, GoapBehaviour goap = null, AgentBrain brain = null)
    {
        OnBind(viewModel, goap, brain);

        transform.position = ViewModel.Position.Value;
    }
    protected abstract void OnBind(CreatureViewModel viewModel, GoapBehaviour goap, AgentBrain brain);
    public void OnPointerClick(PointerEventData eventData)
    {
        ViewModel.OnClick(eventData);
    }
}