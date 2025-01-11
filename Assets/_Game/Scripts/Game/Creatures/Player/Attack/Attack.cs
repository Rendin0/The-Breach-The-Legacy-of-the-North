using UnityEngine;

public class Attack : MonoBehaviour
{
    public float duration = 1f;

    void Update()
    {
        duration -= Time.deltaTime;
        if (duration < 0f)
            Destroy(gameObject);
    }
}
