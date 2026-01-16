using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IPooledObject<T>
{
    [Header("Spawner settings")]
    [SerializeField] private T _prefab;
    [Tooltip("Можно оставить это поле пустым, тогда скрипт сам попытается найти на сцене нужный контейнер")]
    [SerializeField] private PrefabInstancesContainer<T> _prefabsContainer;

    private IObjectPool<T> _pool;

    protected List<T> ActiveObjects = new List<T>();

    private void Awake()
    {
        SetPrefabsContainerIfItNull();
        _pool = new ObjectPool<T>(OnPoolCreate, OnPoolGet, OnPoolRelease, OnPoolDestroy);
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

    private void SetPrefabsContainerIfItNull()
    {
        if (_prefabsContainer == null)
        {
            Debug.LogWarning($"{nameof(_prefabsContainer)} null. Теперь скрипт попытается найти на сцене подходящий объект");
            _prefabsContainer = FindFirstObjectByType<PrefabInstancesContainer<T>>();

            if (_prefabsContainer == null)
            {
                Debug.LogError($"Не удалось найти на сцене {nameof(_prefabsContainer)}");
            }
            else
            {
                Debug.LogWarning($"<color=green>Успех</color>");
            }
        }
    }

    private T OnPoolCreate()
    {
        T pooleObject = Instantiate(_prefab, _prefabsContainer.transform);

        return pooleObject;
    }

    private void OnPoolGet(T pooledObject)
    {
        ActiveObjects.Add(pooledObject);
        pooledObject.transform.SetParent(_prefabsContainer.transform);
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

    // todo Исправить баг создания юнитов. Вроде, если ресурсов хватает и создаётся юнит, то не всегда убавляются ресурсы,
    // потому что сразу после спавна юнита, при добавлении одного ресурса, спавнится ещё один юнит.

    // todo Исправить логику спавна ресурсов. Теперь они должны спавнится в пределах NavMesh.
}