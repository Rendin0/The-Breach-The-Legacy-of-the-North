using UnityEngine;

public class ParticleContainer : MonoBehaviour
{
    public static ParticleContainer instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

}
