using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private ServiceLocator()
    {

    }

    public static ServiceLocator Current { get; private set; }

    private readonly Dictionary<string, IService> _services = new Dictionary<string, IService>();

    public static void Init()
    {
        Current = new ServiceLocator();
    }

    public void Register<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if (_services.ContainsKey(key))
        {
            Debug.LogError($"This service of type {key} is already registered.");
            return;
        }
        _services.Add(key, service);
    }

    public void Unregister<T>(T service) where T : IService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"This service of type {key} is not registered.");
        }
        _services.Remove(key);
    }

    public T Get<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.LogError($"This service of type {key} is not registered.");
        }

        return (T)_services[key];
    }
}
