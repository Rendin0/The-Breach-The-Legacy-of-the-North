using R3;
using R3.Triggers;
using UnityEngine;

public class CreatureSensor : MonoBehaviour
{
    public Observable<Collider2D> OnCreatureSpotted;
    public Observable<Collider2D> OnCreatureLost;

    private void Awake()
    {
        var sensorPrefab = Resources.Load<Collider2D>("Gameplay/CreatureSensor");

        var sensor = Instantiate(sensorPrefab);
        sensor.transform.SetParent(transform, false);

        OnCreatureSpotted = sensor.OnTriggerEnter2DAsObservable();
        OnCreatureLost = sensor.OnTriggerExit2DAsObservable();
    }
}