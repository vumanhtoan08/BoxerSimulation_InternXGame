using System.Collections.Generic;
using UnityEngine;

public static class ObjectPooling
{
    static Dictionary<string, Queue<Component>> poolDictionary = new();
    private static void AddNewPool(Component newObject)
    {
        if (poolDictionary.ContainsKey(newObject.name)) return; // Đã có pool này
        Queue<Component> newPools = new(); // Chưa có pool
        poolDictionary.Add(newObject.name, newPools);
    }

    public static T GetObject<T>(T _Object) where T : Component
    {
        if (!poolDictionary.ContainsKey(_Object.name)) AddNewPool(_Object);
        T newObject;
        if (poolDictionary[_Object.name].Count > 0)
        {
            newObject = (T)poolDictionary[_Object.name].Dequeue();
            newObject.gameObject.SetActive(true);
        }
        else newObject = Object.Instantiate(_Object).GetComponent<T>();
        return newObject;
    }

    public static T GetObject<T>(T _Object, Transform parent) where T : Component
    {
        if (!poolDictionary.ContainsKey(_Object.name)) AddNewPool(_Object);
        T newObject;
        if (poolDictionary[_Object.name].Count > 0)
        {
            newObject = (T)poolDictionary[_Object.name].Dequeue();
            newObject.gameObject.SetActive(true);
        }
        else newObject = Object.Instantiate(_Object).GetComponent<T>();
        newObject.transform.SetParent(parent);
        return newObject;
    }

    public static T GetObject<T>(T _Object, Vector3 pos) where T : Component
    {
        if (!poolDictionary.ContainsKey(_Object.name)) AddNewPool(_Object);
        T newObject;
        if (poolDictionary[_Object.name].Count > 0)
        {
            newObject = (T)poolDictionary[_Object.name].Dequeue();
            newObject.gameObject.SetActive(true);
        }
        else newObject = Object.Instantiate(_Object).GetComponent<T>();
        newObject.transform.position = pos;
        return newObject;
    }

    public static T GetObject<T>(T _Object, Vector3 pos, Transform parent) where T : Component
    {
        T newObject = GetObject(_Object, parent);
        newObject.transform.position = pos;
        return newObject;
    }

    public static T GetObject<T>(T _Object, Vector3 pos, Quaternion rotation) where T : Component
    {
        T newObject = GetObject(_Object);
        newObject.transform.SetPositionAndRotation(pos, rotation);
        return newObject;
    }

    public static T GetObject<T>(T _Object, Vector3 pos, Quaternion rotation, Transform parent) where T : Component
    {
        T newObject = GetObject<T>(_Object, parent);
        newObject.transform.SetPositionAndRotation(pos, rotation);
        return newObject;
    }

    public static void ReturnObject(Component _Object)
    {
        _Object.gameObject.SetActive(false);
        poolDictionary[_Object.name.Replace("(Clone)", "").Trim()].Enqueue(_Object);
    }

    public static void ClearPool()
    {
        poolDictionary = new();
    }
}
