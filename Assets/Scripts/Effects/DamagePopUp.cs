using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] private AnimationCurve opacityCurve;
    [SerializeField] private AnimationCurve heightCurve;

    private TextMeshProUGUI text;
    private float timer = 0;
    private Vector3 origin;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        origin = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;
        text.color = new Color(text.color.r, text.color.g, text.color.b, opacityCurve.Evaluate(timer));
        transform.position = origin + new Vector3(0, heightCurve.Evaluate(timer), 0);
    }
}
