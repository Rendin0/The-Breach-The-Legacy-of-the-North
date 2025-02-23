
using R3;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBinder : MonoBehaviour
{
    [SerializeField] private Transform _selectedItemContainer;
    [SerializeField] private InventorySlotBinder _slotPrefab;

    //[SerializeField] private Button _equipmentButton;
    [SerializeField] private Button _sortInventoryButton;

    [SerializeField] private StorageBinder _mainStorage;
    [SerializeField] private StorageBinder _storagePrefab;
    private StorageBinder _tmpStorage;

    private InventorySlotBinder _selectedItem;
    private readonly Vector3 _offsetSelectedItem = new(10f, -15f);

    private readonly CompositeDisposable _disposables = new();
    private PopupInventoryViewModel _viewModel;
    private PopupInventoryBinder _parrent;

    private void Awake()
    {
        InitSelectedItem();

        _tmpStorage = Instantiate(_storagePrefab, transform);
        _tmpStorage.gameObject.SetActive(false);
    }

    protected void Start()
    {
        //_equipmentButton.onClick.AddListener(OnEquipmentButtonClicked);
        _sortInventoryButton.onClick.AddListener(OnSortButtonClicked);
    }

    private void OnEquipmentButtonClicked()
    {
        _parrent.ToggleEquipment();
    }
    
    private void InitSelectedItem()
    {
        _selectedItem = Instantiate(_slotPrefab, _selectedItemContainer);
        _selectedItem.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.6f);
        _selectedItem.RectTransform.pivot = new Vector2(0f, 1f);
        _selectedItem.interactable = false;
        _selectedItem.Image.raycastTarget = false;
        _selectedItem.Amount.raycastTarget = false;
        _selectedItem.gameObject.SetActive(false);
    }

    private void Update()
    {
        _selectedItem.transform.position = Input.mousePosition + _offsetSelectedItem;
    }

    protected void OnDestroy()
    {
        _sortInventoryButton.onClick.RemoveAllListeners();
        //_equipmentButton.onClick.RemoveAllListeners();
        _disposables.Dispose();
    }

    public void Bind(PopupInventoryViewModel viewModel, PopupInventoryBinder parrent)
    {
        _parrent = parrent;
        _viewModel = viewModel;
        _mainStorage.Bind(viewModel.Storage);

        _disposables.Add(
        viewModel.SelectedChanged.Subscribe(s =>
        {
            SelectedChanged(s);
        }));

        _disposables.Add(
        viewModel.TmpStorage.Subscribe(s =>
        {
            TmpStorageChanged(s);
        }));

    }

    private void SelectedChanged(InventorySlotViewModel slotViewModel)
    {
        if (_selectedItem != null)
        {
            _selectedItem.gameObject.SetActive(false);
        }

        if (slotViewModel != null && slotViewModel.ItemId.Value != ItemsIDs.Nothing)
        {
            _selectedItem.gameObject.SetActive(true);
            _selectedItem.Image.sprite = Resources.Load<Sprite>($"UI/Items/{slotViewModel.ItemId.Value}");
            _selectedItem.Amount.text = slotViewModel.Amount.ToString();

            var slotSize = _mainStorage.GetSlotSize();
            _selectedItem.RectTransform.sizeDelta = slotSize * 0.7f;
        }
    }

    private void TmpStorageChanged(StorageViewModel storage)
    {
        if (storage != null)
            _tmpStorage.Bind(storage);

        _tmpStorage.gameObject.SetActive(storage != null);
    }

    // При взаимодействии с инвентарём через дев панель
    // Сортировка может удалить предметы с неполным стаком
    private void OnSortButtonClicked()
    {
        _viewModel.RequestSortInventory();
    }
}

