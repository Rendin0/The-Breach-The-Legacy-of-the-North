
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMapBinder : MonoBehaviour, IDraggable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _worldLevel;
    [SerializeField] private GameObject _reliefLevel;
    [SerializeField] private GameObject _locationLevel;
    [SerializeField] private GameObject _townLevel;

    private RectTransform _rect;
    private Vector3 _draggingOffset;
    private bool _dragging = false;
    private bool _scalable = false;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_dragging)
            _rect.position = Input.mousePosition + _draggingOffset;
    }

    public void Init(float scale)
    {
        var tmp = _scalable;
        _scalable = true;
        SetScale(scale);
        _scalable = tmp;
    }

    public void SetScale(float scale)
    {
        if (!_scalable)
            return;

        ResizeObject(scale);

        if (scale >= .9f)
            SetWorldScale();
        else if (scale >= .4f)
            SetReliefScale();
        else if (scale >= .2f)
            SetLocationScale();
        else
            SetTownScale();
    }

    private void ResizeObject(float scale)
    {
        var mousePos = Input.mousePosition;
        _rect.pivot = new Vector2(mousePos.x / Screen.width, mousePos.y /  Screen.height);
        var localScale = 1f / scale;
        _rect.localScale = new Vector3(localScale, localScale, localScale);

        //_rect.pivot = new Vector2(.5f, .5f);
    }

    private void SetWorldScale()
    {
        _worldLevel.SetActive(true);
        _reliefLevel.SetActive(false);
        _locationLevel.SetActive(false);
        _townLevel.SetActive(false);
    }
    private void SetReliefScale()
    {
        _worldLevel.SetActive(false);
        _reliefLevel.SetActive(true);
        _locationLevel.SetActive(false);
        _townLevel.SetActive(false);
    }
    private void SetLocationScale()
    {
        _worldLevel.SetActive(false);
        _reliefLevel.SetActive(true);
        _locationLevel.SetActive(true);
        _townLevel.SetActive(false);
    }
    private void SetTownScale()
    {
        _worldLevel.SetActive(false);
        _reliefLevel.SetActive(true);
        _locationLevel.SetActive(false);
        _townLevel.SetActive(true);
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

    public void OnPointerExit(PointerEventData eventData)
    {
        _scalable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _scalable = true;
    }
}