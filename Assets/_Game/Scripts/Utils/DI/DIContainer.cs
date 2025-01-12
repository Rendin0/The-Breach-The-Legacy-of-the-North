using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DIContainer
{
    private readonly DIContainer _parrent;
    private readonly Dictionary<(string, Type), DIRegistration> _registrations = new();
    private readonly HashSet<(string, Type)> _resolutions = new();

    public DIContainer(DIContainer parrent)
    {
        this._parrent = parrent;
    }

    public void RegisterSingleton<T>(Func<DIContainer, T> factory)
    {
        RegisterSingleton(null, factory);
    }
    public void RegisterSingleton<T>(string tag, Func<DIContainer, T> factory)
    {
        Register((tag, typeof(T)), factory, true);
    }
    public void RegisterTransient<T>(Func<DIContainer, T> factory)
    {
        RegisterTransient(null, factory);
    }
    public void RegisterTransient<T>(string tag, Func<DIContainer, T> factory)
    {
        Register((tag, typeof(T)), factory, false);
    }
    public void RegisterInstance<T>(T instance) 
    {
        RegisterInstance(null, instance);    
    }
    public void RegisterInstance<T>(string tag, T instance) 
    {
        var key = (tag, typeof(T));

        if (_registrations.ContainsKey(key))
            throw new Exception($"Factory with tag {key.Item1} and type {key.Item2.FullName} already exitst");

        _registrations[key] = new DIRegistration
        {
            Instance = instance,
            IsSingleton = true
        };
    }
    private void Register<T>((string, Type) key, Func<DIContainer, T> factory, bool isSingleton)
    {
        if (_registrations.ContainsKey(key))
            throw new Exception($"Factory with tag {key.Item1} and type {key.Item2.FullName} already exitst");

        _registrations[key] = new DIRegistration
        {
            Factory = c => factory(c),
            IsSingleton = isSingleton
        };


    }

    public T Resolve<T>(string tag = null)
    {
        var key = (tag, typeof(T));

        if (_resolutions.Contains(key))
            throw new Exception($"Cyclic dependency with tag {tag} and type {key.Item2.FullName}");

        _resolutions.Add(key);

        var resolveResult = TryResolve<T>(key);

        _resolutions.Remove(key);

        return resolveResult;
    }

    private T TryResolve<T>((string, Type) key)
    {
        if (_registrations.TryGetValue(key, out var registration))
        {
            if (registration.IsSingleton)
            {
                if (registration.Instance == null && registration.Factory != null)
                {
                    registration.Instance = registration.Factory(this);
                }

                return (T)registration.Instance;
            }

            return (T)registration.Factory(this);
        }

        if (_parrent != null)
        {
            return _parrent.Resolve<T>(key.Item1);
        }

        throw new Exception($"Couldnt find dependency with tag {key.Item1} and type {key.Item2.FullName}");

    }

}
