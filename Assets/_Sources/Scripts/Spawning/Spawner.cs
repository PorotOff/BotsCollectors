using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IPooledObject<T>
{
    [Header("Spawner settings")]
    [SerializeField] private T _prefab;
    [SerializeField] private Transform _prefabsContainer;

    private IObjectPool<T> _pool;

    protected List<T> ActiveObjects = new List<T>();

    private void Awake()
    {
        _pool = new ObjectPool<T>(OnPoolCreate, OnPoolGet, OnPoolRelease, OnPoolDestroy);
    }

    public void Initialize(Transform prefabsContainer)
    {
        _prefabsContainer = prefabsContainer;
    }

    public virtual void ReleaseAll()
    {
        for (int i = 0; i < ActiveObjects.Count; i++)
        {
            ActiveObjects[i].Release();
        }
    }

    public virtual T Spawn()
    {
        T pooleObject = _pool.Get();
        pooleObject.Released += OnPooledObjectReleased;

        return pooleObject;
    }

    protected virtual void OnPooledObjectReleased(T pooledObject)
    {
        pooledObject.Released -= OnPooledObjectReleased;
        _pool.Release(pooledObject);
    }

    private T OnPoolCreate()
    {
        T pooleObject = Instantiate(_prefab, _prefabsContainer);

        return pooleObject;
    }

    private void OnPoolGet(T pooledObject)
    {
        ActiveObjects.Add(pooledObject);
        pooledObject.transform.SetParent(_prefabsContainer);
        pooledObject.gameObject.SetActive(true);
    }

    private void OnPoolRelease(T pooledObject)
    {
        ActiveObjects.Remove(pooledObject);
        pooledObject.gameObject.SetActive(false);
    }

    private void OnPoolDestroy(T pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}