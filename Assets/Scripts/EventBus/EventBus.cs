using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomEventBus
{
    public class EventBus : IService
    {
        private Dictionary<string, List<object>> _signalCallbacks = new Dictionary<string, List<object>>();

        public void Subscride<T>(Action<T> callback)
        {
            string key = typeof(T).Name;
            if (_signalCallbacks.ContainsKey(key))
            {
                _signalCallbacks[key].Add(callback);
            }
            else
            {
                _signalCallbacks.Add(key, new List<object>() { callback });
            }
        }

        public void Unsubscribe<T>()
        {
            string key = typeof(T).Name;
            if (_signalCallbacks.ContainsKey(key))
            {
                _signalCallbacks.Remove(key);
            }
            else
            {
                Debug.LogError($"{key} is not subscribed.");
            }
        }

        public void Invoke<T>(T signal)
        {
            string key = typeof(T).Name;
            if (_signalCallbacks.ContainsKey(key))
            {
                foreach (var obj in _signalCallbacks[key])
                {
                    var callback = obj as Action<T>;
                    callback?.Invoke(signal);
                }
            }
        }
    }
}
