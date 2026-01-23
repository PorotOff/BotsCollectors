using System;
using UnityEngine;

[RequireComponent(typeof(KillerComponentTriggerDetector))]
public class Flag : MonoBehaviour, IPooledObject<Flag>, ILostable
{
    private KillerComponentTriggerDetector _killerComponentTriggerDetector;

    public event Action<Flag> Released;
    public event Action Replaced;
    public event Action Lost;

    private void Awake()
    {
        _killerComponentTriggerDetector = GetComponent<KillerComponentTriggerDetector>();
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

    public void PlaceAtPosition(Vector3 position)
    {
        Vector3 newPosition = new Vector3(position.x, transform.position.y, position.z);
        transform.position = newPosition;

        Replaced?.Invoke();
    }

    public void PickUp()
    {
        Release();
    }

    public void OnKillerDetected(IKiller killer)
    {
        Lost?.Invoke();
        Release();
    }
}