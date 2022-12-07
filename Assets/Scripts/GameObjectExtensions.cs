using System;
using UnityEngine;

namespace Extensions
{
    static class GameObjectExtensions
    {
        public static void HandleComponent<T>(this GameObject gameObject, Action<T> handler)
        {
            if (gameObject == null) return;

            if (gameObject.TryGetComponent<T>(out var component))
                handler?.Invoke(component);
        }
    }
}