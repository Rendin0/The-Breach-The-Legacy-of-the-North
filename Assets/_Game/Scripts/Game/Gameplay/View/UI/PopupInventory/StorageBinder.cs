using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageBinder : MonoBehaviour, IDraggable
{
    [SerializeField] private InventoryPageBinder _pagePrefab;
    [SerializeField] private Transform _pageContainer;

    [SerializeField] private Button _prevPage;
    [SerializeField] private Button _nextPage;

    [SerializeField] private TMP_Text _currPageText;
    [SerializeField] private TMP_Text _maxPagesText;

    private readonly List<InventorySlotBinder> _slots = new();
    private readonly List<InventoryPageBinder> _slotsPages = new();

    private RectTransform _rect;
    private bool _dragging = false;
    private Vector3 _draggingOffset;

    private int _currentPage = 0;
    private int _maxPages;
    private readonly int _slotsPerPage = 25;

    protected void Start()
    {
        _nextPage.onClick.AddListener(OnNextPageButtonClicked);
        _prevPage.onClick.AddListener(OnPreviousPageButtonClicked);
        _rect = GetComponent<RectTransform>();
    }
    protected void OnDestroy()
    {
        _nextPage.onClick.RemoveAllListeners();
        _prevPage.onClick.RemoveAllListeners();
    }
    private void Update()
    {
        if (_dragging)
            _rect.position = Input.mousePosition + _draggingOffset;
    }

    public void Bind(StorageViewModel viewModel)
    {
        if (viewModel == null)
            return;

        _maxPages = Mathf.CeilToInt(viewModel.Slots.Count / (float)_slotsPerPage);
        _maxPagesText.text = _maxPages.ToString();

        for (int i = 0; i < _maxPages; i++)
        {
            var slotsPage = Instantiate(_pagePrefab, _pageContainer);
            for (int j = 0; j < _slotsPerPage; j++)
            {
                var slot = slotsPage.Slots[j];
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
            pageObject.gameObject.SetActive(false);

        _slotsPages[page].gameObject.SetActive(true);
    }

    public Vector2 GetSlotSize()
    {
        return _slots[0].RectTransform.rect.size;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        _dragging = true;
        _draggingOffset = _rect.position - Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _dragging = false;
    }
}

