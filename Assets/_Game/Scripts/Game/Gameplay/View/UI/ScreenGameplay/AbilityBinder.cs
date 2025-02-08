
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class AbilityBinder : MonoBehaviour
{
    private Image _image;
    [SerializeField] private TMP_Text _cooldownText;

    private CompositeDisposable _subs = new();

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnDestroy()
    {
        _subs.Dispose();
    }

    public void Bind(Ability ability)
    {
        ability.CurrentCooldown.Subscribe(t =>
        {
            ChangeCooldown(t);
        }).AddTo(_subs);

        _image.sprite = Resources.Load<Sprite>($"UI/Abilities/{ability.Name}");

    }

    private void ChangeCooldown(float cooldown)
    {
        _cooldownText.text = cooldown <= 0f ? "" : cooldown.ToString("0.0");
    }
}