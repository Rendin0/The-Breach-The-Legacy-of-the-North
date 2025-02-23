
using R3;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilitiesBarBinder : MonoBehaviour
{
    [SerializeField] private AbilityBinder _abilityPrefab;
    [SerializeField] private List<AbilityBinder> _abilities = new();
    [SerializeField] private GameObject _extraBar;

    public void Bind(AbilitiesBarViewModel viewModel)
    {
        for (int i = 0; i < viewModel.Abilities.Count; i++)
        {
            _abilities[i].Bind(viewModel.Abilities[i]);
        }

        for (int i = _abilities.Count - viewModel.Abilities.Count; i < _abilities.Count; i++)
        {
            _abilities[i].gameObject.SetActive(false);
        }

        viewModel.SwitchBackground.Subscribe(_ => SwitchBackground(_));

        _extraBar.SetActive(false);
    }

    private void SwitchBackground(InputAction.CallbackContext context)
    {
        _extraBar.SetActive(!context.canceled);
    }
}