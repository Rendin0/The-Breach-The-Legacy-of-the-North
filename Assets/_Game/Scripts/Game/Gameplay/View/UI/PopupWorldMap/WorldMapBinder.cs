using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldMapBinder : MonoBehaviour, IDraggable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _worldLevel;
    [SerializeField] private GameObject _reliefLevel;
    [SerializeField] private GameObject _locationLevel;
    [SerializeField] private GameObject _townLevel;

    [SerializeField] private Button _levelsToggleButon;
    [SerializeField] private GameObject _locationsLevels;


    private Canvas _canvas;
    [HideInInspector] public RectTransform Rect;
    private RectTransform _parrentRect;
    private Vector3 _draggingOffset;
    private bool _dragging = false;
    private bool _scalable = false;

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
        _parrentRect = Rect.parent.GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        _levelsToggleButon.onClick.AddListener(ToggleLevels);
        ToggleLevels();
    }

    private void OnDestroy()
    {
        _levelsToggleButon.onClick.RemoveAllListeners();

        
    }

    private void Update()
    {
        if (_dragging)
        {
            Rect.position = Input.mousePosition + _draggingOffset;
            KeepOutsideOfBounds();
        }
    }

    public void Init(float scale, Vector2 position)
    {
        var tmp = _scalable;
        _scalable = true;
        SetScale(scale);
        _scalable = tmp;

        Rect.position = position;
        KeepOutsideOfBounds();
    }
    private void ToggleLevels()
    {
        _locationsLevels.SetActive(!_locationsLevels.activeSelf);
    }

    #region Resizing
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
    private void ResizeObject(float scale)
    {
        // Увеличивает объект относительно позиции курсора

        // Получаем позицию курсора
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Rect,
            Input.mousePosition,
            null,
            out var mousePosition
        );

        var prevScale = Rect.localScale.x;
        var prevPosition = Rect.localPosition;
        Rect.localPosition = Vector3.zero;

        float localScale = 1f / scale;
        Rect.localScale = new Vector3(localScale, localScale, localScale);

        var translatedPosition = (Rect.localPosition - (Vector3)mousePosition) * (localScale - prevScale);

        Rect.localPosition = translatedPosition + prevPosition;
    }
    private void KeepOutsideOfBounds()
    {
        Vector2 distanceToEdge = _parrentRect.position - Rect.position;
        Vector2 maxAllowedDistance = (Rect.rect.size / 2f * Rect.localScale.x * _canvas.scaleFactor) - (Rect.rect.size / 2f * _canvas.scaleFactor);
        Vector2 distanceToMove = distanceToEdge.Abs() - maxAllowedDistance;
        Vector2 moveDirrection = new Vector2(distanceToEdge.normalized.x, 0f).normalized + new Vector2(0f, distanceToEdge.normalized.y).normalized;

        if (distanceToMove.x > 0)
        {
            Rect.position += new Vector3(distanceToMove.x * moveDirrection.x, 0f);
        }
        if (distanceToMove.y > 0)
        {
            Rect.position += new Vector3(0f, distanceToMove.y * moveDirrection.y);
        }

    }
    #endregion

    #region Scale Levels
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
    #endregion

    #region Pointer Events
    public void OnPointerDown(PointerEventData eventData)
    {
        _dragging = true;
        _draggingOffset = Rect.position - Input.mousePosition;
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
    #endregion
}