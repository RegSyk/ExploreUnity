using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Explore
{
    public static class GameObjectExtensions
    {
        public static GameObject AddMeshCollider(this GameObject obj)
        {
            if(obj.TryGetComponent<MeshCollider>(out _))
            {
                throw new InvalidOperationException("MeshCollider is already present");
            }
            obj.AddComponent<MeshCollider>();
            return obj;
        }
        public static T FindSingletonObjectOfType<T>() where T : Object
        {
            T[] objects = Object.FindObjectsOfType<T>();
#if UNITY_EDITOR
            if (objects.Length > 1)
            {
                throw new InvalidOperationException($"Singleton error: objects found ({objects.Length})");
            }
            else if (objects.Length == 0)
            {
                throw new InvalidOperationException($"Singleton error: object not found");
            }
#endif
            return objects[0];
        }
    }
}