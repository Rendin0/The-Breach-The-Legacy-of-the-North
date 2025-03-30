using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ����� ��� ���������� ������ ����, ������� ��������������� � ��������������.
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
    /// ������������� �����������.
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
    /// �������� ���� ���������� ��� ����������� �������.
    /// </summary>
    private void OnDestroy()
    {
        _levelsToggleButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// ���������� ������� ����� ��� ��������������.
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
    /// ������������� ����� � �������� ��������� � ��������.
    /// </summary>
    /// <param name="scale">������� �����.</param>
    /// <param name="position">������� �����.</param>
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
    /// ������������ ��������� ������� �������.
    /// </summary>
    private void ToggleLevels()
    {
        _locationsLevels.SetActive(!_locationsLevels.activeSelf);
    }

    #region Resizing
    /// <summary>
    /// ��������� �������� �����.
    /// </summary>
    /// <param name="scale">������� �����.</param>
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
    /// ��������� ������� ������� ������������ ������� �������.
    /// </summary>
    /// <param name="scale">������� �������.</param>
    private void ResizeObject(float scale)
    {
        // �������������� ������� ������� � ��������� ������� ������ ��������������
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Rect,
            Input.mousePosition,
            null,
            out var mousePosition
        );

        // ���������� ����������� �������� � �������
        var prevScale = Rect.localScale.x;
        var prevPosition = Rect.localPosition;
        Rect.localPosition = Vector3.zero;

        // ��������� ������ ��������
        float localScale = 1f / scale;
        Rect.localScale = new Vector3(localScale, localScale, localScale);

        // ���������� ����� ������� � ������ ��������� ��������
        var translatedPosition = (Rect.localPosition - (Vector3)mousePosition) * (localScale - prevScale);

        // ��������� ����� �������
        Rect.localPosition = translatedPosition + prevPosition;
    }

    /// <summary>
    /// ��������� ����� �� ��������� ������ ������������� �������.
    /// </summary>
    private void KeepOutsideOfBounds()
    {
        // ��������� ���������� �� ������ ������������� ������� �� ������ �����
        Vector2 distanceToEdge = _parentRect.position - Rect.position;

        // ��������� ����������� ���������� ����������, �� ������� ����� ����� ���� �������
        Vector2 maxAllowedDistance = (Rect.rect.size / 2f * Rect.localScale.x * _canvas.scaleFactor) - (Rect.rect.size / 2f * _canvas.scaleFactor);

        // ��������� ����������, �� ������� ����� ����������� �����, ����� ��� ���������� � �������� ������
        Vector2 distanceToMove = distanceToEdge.Abs() - maxAllowedDistance;

        // ���������� ����������� ����������� �����
        Vector2 moveDirrection = new Vector2(distanceToEdge.normalized.x, 0f).normalized + new Vector2(0f, distanceToEdge.normalized.y).normalized;

        // ���� ����� ������� �� ������� �� ��� X, ���������� � �������
        if (distanceToMove.x > 0)
        {
            Rect.position += new Vector3(distanceToMove.x * moveDirrection.x, 0f);
        }

        // ���� ����� ������� �� ������� �� ��� Y, ���������� � �������
        if (distanceToMove.y > 0)
        {
            Rect.position += new Vector3(0f, distanceToMove.y * moveDirrection.y);
        }
    }
    #endregion

    #region Scale Levels
    /// <summary>
    /// ��������� �������� ������ ����.
    /// </summary>
    private void SetWorldScale()
    {
        _worldLevel.SetActive(true);
        _reliefLevel.SetActive(false);
        _locationLevel.SetActive(false);
        _townLevel.SetActive(false);
    }

    /// <summary>
    /// ��������� �������� ������ �������.
    /// </summary>
    private void SetReliefScale()
    {
        _worldLevel.SetActive(true);
        _reliefLevel.SetActive(true);
        _locationLevel.SetActive(false);
        _townLevel.SetActive(false);
    }

    /// <summary>
    /// ��������� �������� ������ �������.
    /// </summary>
    private void SetLocationScale()
    {
        _worldLevel.SetActive(false);
        _reliefLevel.SetActive(true);
        _locationLevel.SetActive(true);
        _townLevel.SetActive(false);
    }

    /// <summary>
    /// ��������� �������� ������ ������.
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
    /// ��������� ������� ������� �� ���������.
    /// </summary>
    /// <param name="eventData">������ ������� ���������.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        _dragging = true;
        _draggingOffset = Rect.position - Input.mousePosition;
    }

    /// <summary>
    /// ��������� ������� ���������� ���������.
    /// </summary>
    /// <param name="eventData">������ ������� ���������.</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        _dragging = false;
    }

    /// <summary>
    /// ��������� ������� ������ ���������.
    /// </summary>
    /// <param name="eventData">������ ������� ���������.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        _scalable = false;
    }

    /// <summary>
    /// ��������� ������� ����� ���������.
    /// </summary>
    /// <param name="eventData">������ ������� ���������.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        _scalable = true;
    }
    #endregion
}
