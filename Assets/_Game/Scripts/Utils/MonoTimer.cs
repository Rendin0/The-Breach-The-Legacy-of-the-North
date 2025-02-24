using System.Collections.Generic;
using UnityEngine;

public class MonoTimer : MonoBehaviour
{
    public static MonoTimer Instance { get; private set; }

    private readonly List<TimerObject> _timers = new();
    private readonly List<TimerObject> _timersToRemove = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        foreach (TimerObject timer in _timers)
            if (!timer.Tick(Time.deltaTime))
                _timersToRemove.Add(timer);

        foreach (var timer in _timersToRemove)
            RemoveTimer(timer);
        _timersToRemove.Clear();
    }

    public void AddTimer(TimerObject timer)
    {
        _timers.Add(timer);
    }

    private void RemoveTimer(TimerObject timer)
    {
        _timers.Remove(timer);
    }

}