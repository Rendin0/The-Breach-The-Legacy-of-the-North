using CrashKonijn.Agent.Runtime;
using R3;
using R3.Triggers;
using UnityEditor.VersionControl;
using UnityEngine;

public class CreatureSensor : MonoBehaviour
{
    public Subject<Collider2D> OnEnemySpotted = new();
    public Subject<Collider2D> OnCreatureLost = new();

    private LayerMask _enemies;
    private void Awake()
    {
        var sensorPrefab = Resources.Load<Collider2D>("Gameplay/CreatureSensor");

        var sensor = Instantiate(sensorPrefab);
        sensor.transform.SetParent(transform, false);

        _enemies = GetComponent<CreatureBinder>().ViewModel.Enemies;

        sensor.OnTriggerEnter2DAsObservable().Subscribe(c => OnCreatureSpotted(c));
        sensor.OnTriggerExit2DAsObservable().Subscribe(c => OnCreatureLostE(c));
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

    private void OnCreatureLostE(Collider2D collider)
    {
        OnCreatureLost.OnNext(collider);
    }
}