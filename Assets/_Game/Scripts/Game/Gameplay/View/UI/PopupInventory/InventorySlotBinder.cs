
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Profiling.HierarchyFrameDataView;

public class InventorySlotBinder : Selectable, IPointerDownHandler, IElementInfoBinder
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _rarityImage;
    [SerializeField] private TMP_Text _amount;
    public Image Image { get { return _icon; } }
    public Image Rarity { get { return _rarityImage; } }
    public TMP_Text Amount { get { return _amount; } }

    private InventorySlotViewModel _viewModel;
    private readonly CompositeDisposable _subs = new();


    private RectTransform _rectTransorfm;
    public RectTransform RectTransform { get { return _rectTransorfm; } }

    protected override void Awake()
    {
        base.Awake();
        _rectTransorfm = GetComponent<RectTransform>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Image.color = Color.clear;
        _rarityImage.color = Color.clear;
        Amount.color = Color.clear;
        _viewModel.OnMouseExit.OnNext(_viewModel);
        _viewModel.RequestSelect();
    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _subs.Dispose();
    }

    public void Bind(InventorySlotViewModel viewModel)
    {
        _viewModel = viewModel;

        var itemAmountChangedSub =
            viewModel.Amount.Subscribe(amount =>
            {
                ChangeAmount(amount);
            });
        var itemIdChangedSub =
            viewModel.ItemId.Subscribe(id =>
            {
                ChangeImage(id);
            });
        var resetColorSub =
            viewModel.ResetColor.Subscribe(_ =>
            {
                if (_icon.sprite != null)
                {
                    Image.color = Color.white;
                    Amount.color = Color.white;
                    _rarityImage.color = Color.white;
                }
            });


        _subs.Add(itemAmountChangedSub);
        _subs.Add(itemIdChangedSub);
        _subs.Add(resetColorSub);

    }

    private void ChangeAmount(int amount)
    {
        _amount.color = Color.white;
        if (amount > 1)
        {
            _amount.text = amount.ToString();
            _rarityImage.sprite = Resources.Load<Sprite>($"UI/Items/Rarities/Rarity{_viewModel.Rarity}S");
        }
        else
        {
            _amount.text = "";
            _rarityImage.sprite = Resources.Load<Sprite>($"UI/Items/Rarities/Rarity{_viewModel.Rarity}");
        }
    }

    private void ChangeImage(string id)
    {
        var sprite = Resources.Load<Sprite>($"UI/Items/{id}");

        _icon.sprite = sprite;
        _viewModel.ItemIcon = sprite;

        if (sprite == null)
        {
            _rarityImage.color = Color.clear;
            _icon.color = Color.clear;
            return;
        }
        _rarityImage.color = Color.white;
        _icon.color = Color.white;
    }



    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (_viewModel.ItemId.Value != ItemsIDs.Nothing && _icon.color != Color.clear)
            _viewModel.OnMouseEnter.OnNext(_viewModel);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (_viewModel.ItemId.Value != ItemsIDs.Nothing)
            _viewModel.OnMouseExit.OnNext(_viewModel);
    }
}