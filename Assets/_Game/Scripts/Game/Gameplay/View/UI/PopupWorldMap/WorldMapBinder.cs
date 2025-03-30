using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Класс для управления картой мира, включая масштабирование и перетаскивание.
/// </summary>
public class WorldMapBinder : MonoBehaviour, IDraggable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _worldLevel;
    [SerializeField] private GameObject _reliefLevel;
    [SerializeField] private GameObject _locationLevel;
    [SerializeField] private GameObject _townLevel;

    [SerializeField] private Button _levelsToggleButton;
    [SerializeField] private GameObject _locationsLevels;

    private Canvas _canvas;
    [HideInInspector] public RectTransform Rect;
    private RectTransform _parentRect;
    private Vector3 _draggingOffset;
    private bool _dragging = false;
    private bool _scalable = false;

    /// <summary>
    /// Инициализация компонентов.
    /// </summary>
    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
        _parentRect = Rect.parent.GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();

        _levelsToggleButton.onClick.AddListener(ToggleLevels);
        ToggleLevels();
    }

    /// <summary>
    /// Удаление всех слушателей при уничтожении объекта.
    /// </summary>
    private void OnDestroy()
    {
        _levelsToggleButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Обновление позиции карты при перетаскивании.
    /// </summary>
    private void Update()
    {
        if (_dragging)
        {
            Rect.position = Input.mousePosition + _draggingOffset;
            KeepOutsideOfBounds();
        }
    }

    /// <summary>
    /// Инициализация карты с заданным масштабом и позицией.
    /// </summary>
    /// <param name="scale">Масштаб карты.</param>
    /// <param name="position">Позиция карты.</param>
    public void Init(float scale, Vector2 position)
    {
        var tmp = _scalable;
        _scalable = true;
        SetScale(scale);
        _scalable = tmp;

        Rect.position = position;
        KeepOutsideOfBounds();
    }

    /// <summary>
    /// Переключение видимости уровней локаций.
    /// </summary>
    private void ToggleLevels()
    {
        _locationsLevels.SetActive(!_locationsLevels.activeSelf);
    }

    #region Resizing
    /// <summary>
    /// Установка масштаба карты.
    /// </summary>
    /// <param name="scale">Масштаб карты.</param>
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

    /// <summary>
    /// Изменение размера объекта относительно позиции курсора.
    /// </summary>
    /// <param name="scale">Масштаб объекта.</param>
    private void ResizeObject(float scale)
    {
        // Преобразование позиции курсора в локальную позицию внутри прямоугольника
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Rect,
            Input.mousePosition,
            null,
            out var mousePosition
        );

        // Сохранение предыдущего масштаба и позиции
        var prevScale = Rect.localScale.x;
        var prevPosition = Rect.localPosition;
        Rect.localPosition = Vector3.zero;

        // Установка нового масштаба
        float localScale = 1f / scale;
        Rect.localScale = new Vector3(localScale, localScale, localScale);

        // Вычисление новой позиции с учетом изменения масштаба
        var translatedPosition = (Rect.localPosition - (Vector3)mousePosition) * (localScale - prevScale);

        // Установка новой позиции
        Rect.localPosition = translatedPosition + prevPosition;
    }

    /// <summary>
    /// Удержание карты за пределами границ родительского объекта.
    /// </summary>
    private void KeepOutsideOfBounds()
    {
        // Вычисляем расстояние от центра родительского объекта до центра карты
        Vector2 distanceToEdge = _parentRect.position - Rect.position;

        // Вычисляем максимально допустимое расстояние, на которое карта может быть смещена
        Vector2 maxAllowedDistance = (Rect.rect.size / 2f * Rect.localScale.x * _canvas.scaleFactor) - (Rect.rect.size / 2f * _canvas.scaleFactor);

        // Вычисляем расстояние, на которое нужно переместить карту, чтобы она оставалась в пределах границ
        Vector2 distanceToMove = distanceToEdge.Abs() - maxAllowedDistance;

        // Определяем направление перемещения карты
        Vector2 moveDirrection = new Vector2(distanceToEdge.normalized.x, 0f).normalized + new Vector2(0f, distanceToEdge.normalized.y).normalized;

        // Если карта выходит за границы по оси X, перемещаем её обратно
        if (distanceToMove.x > 0)
        {
            Rect.position += new Vector3(distanceToMove.x * moveDirrection.x, 0f);
        }

        // Если карта выходит за границы по оси Y, перемещаем её обратно
        if (distanceToMove.y > 0)
        {
            Rect.position += new Vector3(0f, distanceToMove.y * moveDirrection.y);
        }
    }
    #endregion

    #region Scale Levels
    /// <summary>
    /// Установка масштаба уровня мира.
    /// </summary>
    private void SetWorldScale()
    {
        _worldLevel.SetActive(true);
        _reliefLevel.SetActive(false);
        _locationLevel.SetActive(false);
        _townLevel.SetActive(false);
    }

    /// <summary>
    /// Установка масштаба уровня рельефа.
    /// </summary>
    private void SetReliefScale()
    {
        _worldLevel.SetActive(true);
        _reliefLevel.SetActive(true);
        _locationLevel.SetActive(false);
        _townLevel.SetActive(false);
    }

    /// <summary>
    /// Установка масштаба уровня локации.
    /// </summary>
    private void SetLocationScale()
    {
        _worldLevel.SetActive(false);
        _reliefLevel.SetActive(true);
        _locationLevel.SetActive(true);
        _townLevel.SetActive(false);
    }

    /// <summary>
    /// Установка масштаба уровня города.
    /// </summary>
    private void SetTownScale()
    {
        _worldLevel.SetActive(false);
        _reliefLevel.SetActive(true);
        _locationLevel.SetActive(false);
        _townLevel.SetActive(true);
    }
    #endregion

    #region Pointer Events
    /// <summary>
    /// Обработка события нажатия на указатель.
    /// </summary>
    /// <param name="eventData">Данные события указателя.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        _dragging = true;
        _draggingOffset = Rect.position - Input.mousePosition;
    }

    /// <summary>
    /// Обработка события отпускания указателя.
    /// </summary>
    /// <param name="eventData">Данные события указателя.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        _dragging = false;
    }

    /// <summary>
    /// Обработка события выхода указателя.
    /// </summary>
    /// <param name="eventData">Данные события указателя.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        _scalable = false;
    }

    /// <summary>
    /// Обработка события входа указателя.
    /// </summary>
    /// <param name="eventData">Данные события указателя.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        _scalable = true;
    }
    #endregion
}
