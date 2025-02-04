using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageBinder : MonoBehaviour
{
    [SerializeField] private GameObject _pagePrefab;

    [SerializeField] private Button _prevPage;
    [SerializeField] private Button _nextPage;

    [SerializeField] private TMP_Text _currPageText;
    [SerializeField] private TMP_Text _maxPagesText;

    private InventorySlotBinder _slotPrefab;
    private List<InventorySlotBinder> _slots = new();
    private List<GameObject> _slotsPages = new();

    private int _currentPage = 0;
    private int _maxPages;
    private int _slotsPerPage = 16;

    protected void Start()
    {
        _nextPage.onClick.AddListener(OnNextPageButtonClicked);
        _prevPage.onClick.AddListener(OnPreviousPageButtonClicked);
    }
    protected void OnDestroy()
    {
        _nextPage.onClick.RemoveAllListeners();
        _prevPage.onClick.RemoveAllListeners();
    }

    public void Bind(StorageViewModel viewModel, InventorySlotBinder slotPrefab)
    {
        if (viewModel == null)
            return;

        _slotPrefab = slotPrefab;

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
    }

    private void SetPage(int page)
    {
        _currPageText.text = (page + 1).ToString();
        foreach (var pageObject in _slotsPages)
            pageObject.SetActive(false);

        _slotsPages[page].SetActive(true);
    }

    public Vector2 GetSlotSize()
    {
        return _slots[0].RectTransform.sizeDelta;
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
}

