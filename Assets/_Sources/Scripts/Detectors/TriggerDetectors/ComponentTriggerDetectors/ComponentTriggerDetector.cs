using System;
using UnityEngine;

public class ComponentTriggerDetector<T> : MonoBehaviour
{
    public event Action<T> Detected;
    public event Action Lost;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out T component))
        {            
            Detected?.Invoke(component);
        }
    }

    private  void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out T component))
        {
            Lost?.Invoke();
        }
    }
}