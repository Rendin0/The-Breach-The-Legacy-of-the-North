
using System;
using UnityEngine;

public class TimerObject
{
    private float _timer;
    private readonly Action<float, float> _tickAction;
    private readonly Action _endAction;

    public TimerObject(float duration, Action<float, float> tickAction = null, Action endAction = null)
    {
        _timer = duration;
        _tickAction = tickAction;
        _endAction = endAction;
    }

    // Возвращаемое значение
    // true - таймер идёт
    // false - таймер закончился
    public bool Tick(float timeFromLastTick)
    {
        _timer -= timeFromLastTick;
        _tickAction?.Invoke(timeFromLastTick, _timer);

        if (_timer < 0)
        {
            _endAction?.Invoke();
            return false;
        }
        return true;
    }
}