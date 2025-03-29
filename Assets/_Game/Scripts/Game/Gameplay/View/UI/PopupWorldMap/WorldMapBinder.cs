
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class WorldMapBinder : MonoBehaviour, IDraggable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _worldLevel;
    [SerializeField] private GameObject _reliefLevel;
    [SerializeField] private GameObject _locationLevel;
    [SerializeField] private GameObject _townLevel;

    private RectTransform _rect;
    private RectTransform _parrentRect;
    private Vector3 _draggingOffset;
    private bool _dragging = false;
    private bool _scalable = false;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _parrentRect = _rect.parent.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_dragging)
        {
            _rect.position = Input.mousePosition + _draggingOffset;
            KeepOutsideOfBounds();
        }
    }

    public void Init(float scale)
    {
        var tmp = _scalable;
        _scalable = true;
        //SetScale(scale);
        _scalable = tmp;
    }

    public void SetScale(float scale)
    {
        if (!_scalable)
            return;

        ResizeObject(scale);
        KeepOutsideOfBounds();

        if (scale >= .9f)
            SetWorldScale();
        else if (scale >= .4f)
            SetReliefScale();
        else if (scale >= .2f)
            SetLocationScale();
        else
            SetTownScale();
    }

    // Увеличивает объект относительно позиции курсора
    private void ResizeObject(float scale)
    {
        // Получаем позицию курсора
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rect,
            Input.mousePosition,
            null,
            out var mousePosition
        );

        var prevScale = _rect.localScale.x;
        var prevPosition = _rect.localPosition;
        _rect.localPosition = Vector3.zero;

        float localScale = 1f / scale;
        _rect.localScale = new Vector3(localScale, localScale, localScale);

        var translatedPosition = (_rect.localPosition - (Vector3)mousePosition) * (localScale - prevScale);

        _rect.localPosition = translatedPosition + prevPosition;
    }

    private void KeepOutsideOfBounds()
    {

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
        _worldLevel.SetActive(true);
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