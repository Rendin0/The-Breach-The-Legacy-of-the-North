
using UnityEngine;

public class CreatureBinder : MonoBehaviour
{
    public void Bind(CreatureViewModel viewModel)
    {
        transform.position = viewModel.Position.CurrentValue;
    }
}