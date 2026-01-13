using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : Component
{
    private T _prefab;
    private Transform _poolContainer;
    private Queue<T> _poolQueue = new Queue<T>();
    private int _initialSize;

    public ObjectPool(T prefab, int initialSize = 10)
    {
        _prefab = prefab;
        _initialSize = initialSize;
        _poolContainer = new GameObject($"{typeof(T).Name}Pool").transform;
        Preload();
    }

    private void Preload()
    {
        for (int i = 0; i < _initialSize; i++)
        {
            var obj = GameObject.Instantiate(_prefab, _poolContainer);
            obj.gameObject.SetActive(false);
            _poolQueue.Enqueue(obj);
        }
    }

    public T Get(Vector3 position, Quaternion rotation)
    {
        if (_poolQueue.Count == 0)
        {
            var newObj = GameObject.Instantiate(_prefab, position, rotation, _poolContainer);
            return newObj;
        }

        var pooledObj = _poolQueue.Dequeue();
        pooledObj.transform.SetPositionAndRotation(position, rotation);
        pooledObj.gameObject.SetActive(true);
        return pooledObj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        _poolQueue.Enqueue(obj);
    }
}