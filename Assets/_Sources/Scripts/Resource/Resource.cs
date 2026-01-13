using System;
using UnityEngine;

public class Resource : MonoBehaviour, IPooledObject<Resource>
{
    public event Action<Resource> Released;
    public event Action PickedUp;

    private Collider _collider;

    private void Awake()
    {
        if (TryGetComponent(out _collider) == false)
        {
            Debug.Log($"Ошибка получения коллайдера на объекте {name}");
        }
    }

    public void Release()
    {
        Released?.Invoke(this);
    }

    public void Pickup()
    {
        if (_collider != null)
        {
            _collider.enabled = false;
        }

        PickedUp?.Invoke();
    }

    public void Collect()
    {
        if (_collider != null)
        {
            _collider.enabled = true;
        }

        Release();
    }
}