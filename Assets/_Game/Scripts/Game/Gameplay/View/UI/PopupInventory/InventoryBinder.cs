
using R3;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBinder : MonoBehaviour
{
    [SerializeField] private Transform _selectedItemContainer;
    [SerializeField] private InventorySlotBinder _slotPrefab;

    [SerializeField] private Button _equipmentButton;
    [SerializeField] private Button _sortInventoryButton;

    [SerializeField] private StorageBinder _mainStorage;
    [SerializeField] private StorageBinder _storagePrefab;
    private StorageBinder _tmpStorage;

    private InventorySlotBinder _selectedItem;
    private Vector3 _offsetSelectedItem;

    private CompositeDisposable _disposables = new();
    private PopupInventoryViewModel _viewModel;
    private PopupInventoryBinder _parrent;

    private void Awake()
    {
        _selectedItem = Instantiate(_slotPrefab, _selectedItemContainer);
        _selectedItem.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.6f);
        _selectedItem.gameObject.SetActive(false);

        _tmpStorage = Instantiate(_storagePrefab, transform);
        _tmpStorage.gameObject.SetActive(false);
    }

    protected void Start()
    {
        _equipmentButton.onClick.AddListener(OnEquipmentButtonClicked);
        _sortInventoryButton?.onClick.AddListener(OnSortButtonClicked);
    }

    private void OnEquipmentButtonClicked()
    {
        _parrent.ToggleEquipment();
    }

    private void Update()
    {
        _selectedItem.transform.position = Input.mousePosition + _offsetSelectedItem;
    }

    protected void OnDestroy()
    {
        _sortInventoryButton?.onClick.RemoveAllListeners();
        _equipmentButton.onClick.RemoveAllListeners();
        _disposables.Dispose();
    }

    public void Bind(PopupInventoryViewModel viewModel, PopupInventoryBinder parrent)
    {
        _parrent = parrent;
        _viewModel = viewModel;
        _mainStorage.Bind(viewModel.Storage, _slotPrefab);

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
            _selectedItem.RectTransform.sizeDelta = slotSize;
            _offsetSelectedItem = slotSize * 1.1f;
        }
    }

    private void TmpStorageChanged(StorageViewModel storage)
    {
        if (storage != null)
            _tmpStorage.Bind(storage, _slotPrefab);

        _tmpStorage.gameObject.SetActive(storage != null);
    }

    private void OnSortButtonClicked()
    {
        _viewModel.RequestSortInventory();
    }
}

