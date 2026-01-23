using System;
using UnityEngine;

public class TargetTriggerDetector<T> : MonoBehaviour where T : MonoBehaviour
{
    private T _target;

    public event Action<T> Detected;
    public event Action Lost;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out T component))
        {            
            if (_target != null && _target == component)
            {
                Detected?.Invoke(component);
            }
        }
    }

    private  void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out T component))
        {
            if (_target != null && _target == component)
            {
                Lost?.Invoke();
            }
        }
    }

    public void Initialize(T target)
    {
        _target = target;
    }

    public void Deinitialize()
    {
        _target = null;
    }
}