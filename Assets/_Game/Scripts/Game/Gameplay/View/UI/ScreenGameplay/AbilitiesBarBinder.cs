

using System.Collections.Generic;
using UnityEngine;

public class AbilitiesBarBinder : MonoBehaviour
{
    [SerializeField] private AbilityBinder _abilityPrefab;
    private List<AbilityBinder> _abilities = new();


    public void Bind(AbilitiesBarViewModel viewModel)
    {
        foreach (var ability in viewModel.Abilities)
        {
            var abilityBinder = Instantiate(_abilityPrefab, transform);
            abilityBinder.Bind(ability);

            _abilities.Add(abilityBinder);
        }
    }

}