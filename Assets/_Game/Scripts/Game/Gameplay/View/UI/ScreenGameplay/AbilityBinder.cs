
using UnityEngine;
using UnityEngine.UI;

public class AbilityBinder : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Bind(Ability ability)
    {
        _image.sprite = Resources.Load<Sprite>($"UI/Abilities/{ability.Name}");
    }

}