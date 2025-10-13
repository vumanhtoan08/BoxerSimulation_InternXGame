using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
public static class FactoryObject
{
    public const string ObjectPool = "Object";
    public static T Spawn<T>(string poolName, Transform prefab)
    {
        T obj = default;
        obj = PoolManager.Pools[poolName].Spawn(prefab).GetComponent<T>();
        return obj;
    }
    public static T Spawn<T>(string poolName, Transform prefab, Vector3 position, Quaternion rotation)
    {
        T obj = default;
        obj = PoolManager.Pools[poolName].Spawn(prefab, position, rotation).GetComponent<T>();
        return obj;
    }
    public static T Spawn<T>(string poolName, Transform prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        T obj = default;
        obj = PoolManager.Pools[poolName].Spawn(prefab, position, rotation, parent).GetComponent<T>();
        return obj;
    }
    public static T Spawn<T>(string poolName, Transform prefab, Transform parent)
    {
        T obj = default;
        obj = PoolManager.Pools[poolName].Spawn(prefab, parent).GetComponent<T>();
        return obj;
    }

    public static T Spawn<T>(string poolName, string prefabName)
    {
        T obj = default;
        obj = PoolManager.Pools[poolName].Spawn(prefabName).GetComponent<T>();
        return obj;
    }
    public static T Spawn<T>(string poolName, string prefabName, Vector3 position, Quaternion rotation)
    {
        T obj = default;
        obj = PoolManager.Pools[poolName].Spawn(prefabName, position, rotation).GetComponent<T>();
        return obj;
    }
    public static T Spawn<T>(string poolName, string prefabName, Vector3 position, Quaternion rotation, Transform parent)
    {
        T obj = default;
        obj = PoolManager.Pools[poolName].Spawn(prefabName, position, rotation, parent).GetComponent<T>();
        return obj;
    }
    public static T Spawn<T>(string poolName, string prefabName, Transform parent)
    {
        T obj = default;
        obj = PoolManager.Pools[poolName].Spawn(prefabName, parent).GetComponent<T>();
        return obj;
    }
    public static void Despawn(string poolName, Transform obj)
    {
        var pool = PoolManager.Pools[poolName];
        obj.parent = pool.transform;
        pool.Despawn(obj);
    }
    public static void Despawn(string poolName, Transform obj, Transform parent)
    {
        PoolManager.Pools[poolName].Despawn(obj, parent);
    }
    public static void Despawn(string poolName, Transform obj, float delay)
    {
        PoolManager.Pools[poolName].Despawn(obj, delay);
    }
    public static void Despawn(string poolName, Transform obj, float delay, Transform parent)
    {
        PoolManager.Pools[poolName].Despawn(obj, delay, parent);
    }
}
