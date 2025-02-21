

using System.Collections.Generic;
using UnityEngine;

public class AbilitiesBarBinder : MonoBehaviour
{
    [SerializeField] private AbilityBinder _abilityPrefab;
    [SerializeField] private List<AbilityBinder> _abilities = new();


    public void Bind(AbilitiesBarViewModel viewModel)
    {
        for (int i = 0; i < viewModel.Abilities.Count; i++)
        {
            _abilities[i].Bind(viewModel.Abilities[i]);
        }
    }

}