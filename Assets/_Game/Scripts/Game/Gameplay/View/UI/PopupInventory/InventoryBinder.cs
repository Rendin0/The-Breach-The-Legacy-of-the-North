
using R3;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBinder : MonoBehaviour
{
    [SerializeField] private Transform _selectedItemContainer;
    [SerializeField] private GameObject _pagePrefab;
    [SerializeField] private InventorySlotBinder _slotPrefab;

    [SerializeField] private Button _sortInventoryButton;
    [SerializeField] private Button _prevPage;
    [SerializeField] private Button _nextPage;

    [SerializeField] private TMP_Text _currPageText;
    [SerializeField] private TMP_Text _maxPagesText;

    private List<InventorySlotBinder> _slots = new();
    private List<GameObject> _slotsPages = new();
    private CompositeDisposable _disposables = new();

    private int _currentPage = 0;
    private int _maxPages;
    private int _slotsPerPage = 16;

    private InventorySlotBinder _selectedItem;
    private Vector3 _offsetSelectedItem;

    private PopupInventoryViewModel _viewModel;

    protected void Start()
    {
        _sortInventoryButton?.onClick.AddListener(OnSortButtonClicked);
        _nextPage.onClick.AddListener(OnNextPageButtonClicked);
        _prevPage.onClick.AddListener(OnPreviousPageButtonClicked);
    }

    private void Update()
    {
        if (_selectedItem != null)
            _selectedItem.transform.position = Input.mousePosition + _offsetSelectedItem;
    }

    protected void OnDestroy()
    {
        _sortInventoryButton.onClick.RemoveAllListeners();
        _nextPage.onClick.RemoveAllListeners();
        _prevPage.onClick.RemoveAllListeners();
        _disposables.Dispose();

    }
    private void OnSortButtonClicked()
    {
        _viewModel.RequestSortInventory();
    }

    private void OnNextPageButtonClicked()
    {
        _currentPage = (_currentPage + 1) % _maxPages;
        SetPage(_currentPage);
    }
    private void OnPreviousPageButtonClicked()
    {
        _currentPage = _currentPage == 0 ? _maxPages - 1 : _currentPage - 1;
        SetPage(_currentPage);
    }

    private void SetPage(int page)
    {
        _currPageText.text = (page + 1).ToString();
        foreach (var pageObject in _slotsPages)
            pageObject.SetActive(false);

        _slotsPages[page].SetActive(true);
    }


    public void Bind(PopupInventoryViewModel viewModel)
    {
        _viewModel = viewModel;

        _maxPages = Mathf.CeilToInt(viewModel.Slots.Count / (float)_slotsPerPage);
        _maxPagesText.text = _maxPages.ToString();

        for (int i = 0; i < _maxPages; i++)
        {
            var slotsPage = Instantiate(_pagePrefab, transform);
            for (int j = 0; j < _slotsPerPage; j++)
            {
                var slot = Instantiate(_slotPrefab, slotsPage.transform);
                slot.Bind(viewModel.Slots[i * _slotsPerPage + j]);
                _slots.Add(slot);
            }
            _slotsPages.Add(slotsPage);
        }

        SetPage(0);

        _disposables.Add(
        viewModel.SelectedChanged.Subscribe(s =>
        {
            if (_selectedItem != null)
            {
                Destroy(_selectedItem.gameObject);
                _selectedItem = null;
            }

            if (s != -1 && viewModel.Slots[s].ItemId.Value != ItemsTypes.Nothing)
            {
                _selectedItem = Instantiate(_slots[s], _selectedItemContainer);
                _selectedItem.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.6f);
                var rect = _selectedItem.GetComponent<RectTransform>();
                rect.sizeDelta = _slots[s].GetComponent<RectTransform>().sizeDelta;
                _offsetSelectedItem = rect.sizeDelta * 1.1f;
            }
        }));
    }
}

