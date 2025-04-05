using R3;
using R3.Triggers;
using UnityEngine;

public class CreatureSensor : MonoBehaviour
{
    public Subject<Collider2D> OnEnemySpotted { get; } = new();
    public Subject<Collider2D> OnEnemyLost { get; } = new();

    private LayerMask _enemies;
    private void Awake()
    {
        var sensorPrefab = Resources.Load<Collider2D>("Gameplay/CreatureSensor");

        var sensor = Instantiate(sensorPrefab);
        sensor.transform.SetParent(transform, false);

        _enemies = GetComponent<CreatureBinder>().ViewModel.Enemies;

        sensor.OnTriggerEnter2DAsObservable().Subscribe(c => OnCreatureSpotted(c));
        sensor.OnTriggerExit2DAsObservable().Subscribe(c => OnCreatureLost(c));
    }

    private void OnCreatureSpotted(Collider2D collider)
    {
        int layer = collider.gameObject.layer;

        // Проверка на наличие слоя в маске слоёв врагов
        // https://discussions.unity.com/t/checking-if-a-layer-is-in-a-layer-mask/860331/2
        if ((_enemies & (1 << layer)) != 0)
        {
            OnEnemySpotted.OnNext(collider);
        }
    }

    private void OnCreatureLost(Collider2D collider)
    {
        int layer = collider.gameObject.layer;

        // Проверка на наличие слоя в маске слоёв врагов
        // https://discussions.unity.com/t/checking-if-a-layer-is-in-a-layer-mask/860331/2
        if ((_enemies & (1 << layer)) != 0)
        {
            OnEnemyLost.OnNext(collider);
        }
    }
}