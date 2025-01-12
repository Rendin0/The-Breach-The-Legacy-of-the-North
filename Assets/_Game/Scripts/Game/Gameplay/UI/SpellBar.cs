using UnityEngine;
using UnityEngine.UI;

public class SpellBar : MonoBehaviour
{
    public static SpellBar instance { get; private set; }

    public Image[] slots;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            slots = GetComponentsInChildren<Image>();
            return;
        }

        Destroy(gameObject);
    }
}
