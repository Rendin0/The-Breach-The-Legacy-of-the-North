
using R3;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilitiesBarBinder : MonoBehaviour
{
    [SerializeField] private AbilityBinder _abilityPrefab;
    [SerializeField] private List<AbilityBinder> _abilities = new();
    [SerializeField] private GameObject _extraBar;
    [SerializeField] private List<TMP_Text> _abilityBindingsNames;

    public void Bind(AbilitiesBarViewModel viewModel)
    {
        for (int i = 0; i < viewModel.Abilities.Count; i++)
        {
            _abilities[i].Bind(viewModel.Abilities[i]);
        }

        for (int i = viewModel.Abilities.Count; i < _abilities.Count; i++)
        {
            _abilities[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < viewModel.AbilityBindings.Count; i++)
        {
            _abilityBindingsNames[i].text = viewModel.AbilityBindings[i];
        }


        viewModel.AddExtraBar.Subscribe(_ => AddExtraBar(_));
        _extraBar.SetActive(false);
    }

    private void AddExtraBar(InputAction.CallbackContext context)
    {
        _extraBar.SetActive(!context.canceled);
    }
}