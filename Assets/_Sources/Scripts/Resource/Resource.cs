using System;
using UnityEngine;

[RequireComponent(typeof(KillerComponentTriggerDetector))]
public class Resource : MonoBehaviour, IPooledObject<Resource>, ILostable
{
    private Collider _collider;
    private KillerComponentTriggerDetector _killerComponentTriggerDetector;

    public event Action<Resource> Released;
    public event Action Lost;

    private void Awake()
    {
        _killerComponentTriggerDetector = GetComponent<KillerComponentTriggerDetector>();

        if (TryGetComponent(out _collider) == false)
        {
            Debug.Log($"Ошибка получения коллайдера на объекте {name}");
        }
    }

    private void OnEnable()
    {
        _killerComponentTriggerDetector.Detected += OnKillerDetected;
    }

    private void OnDisable()
    {
        _killerComponentTriggerDetector.Detected -= OnKillerDetected;
    }

    public void Release()
    {
        Released?.Invoke(this);
    }

    public void PickUp(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;

        PickUp();
    }

    public void PickUp()
    {
        if (_collider != null)
        {
            _collider.enabled = false;
        }
    }

    public void Collect()
    {
        if (_collider != null)
        {
            _collider.enabled = true;
        }
        
        Release();
    }

    public void OnKillerDetected(IKiller killer)
    {
        Lost?.Invoke();
        Release();
    }
}