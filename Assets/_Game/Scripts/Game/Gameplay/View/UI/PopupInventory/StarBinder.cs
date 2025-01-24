
using R3;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarBinder : MonoBehaviour
{
    private Image _img;
    private RectTransform _rect;

    public ItemRarity Rarity { get; private set; }

    private readonly CompositeDisposable _subs = new();

    public readonly Subject<Unit> StarChanged = new();

    private void Awake()
    {
        _img = GetComponent<Image>();
        _rect = GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        _subs.Dispose();
    }

    public void Bind(InventorySlotViewModel slot, Dictionary<string, ItemConfig> itemsConfig)
    {
        _subs.Add(slot.ItemId.Subscribe(id =>
        {
            Rarity = itemsConfig[id].Rarity;
            SetImage($"UI/Stars/StarSmall{Rarity}");
            StarChanged.OnNext(Unit.Default);
        }));

    }

    public void SetImage(string path)
    {
        _img.sprite = Resources.Load<Sprite>(path);
        _img.SetNativeSize();
        _rect.sizeDelta *= 2;
    }

}