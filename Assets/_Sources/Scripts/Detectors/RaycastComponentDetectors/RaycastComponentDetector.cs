using System;
using UnityEngine;

public class RaycastComponentDetector<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private InputService _inputService;
    [SerializeField, Min(0)] private float _maxRaycastLength = 50f;
    [SerializeField] private LayerMask _layerMask;

    public event Action<T> Detected;

    public Vector3 RaycastHitPosiion { get; private set; }

    private void OnEnable()
    {
        _inputService.Pressed += Detect;
    }

    private void OnDisable()
    {
        _inputService.Pressed -= Detect;
    }

    public void Detect(Vector2 pressPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(pressPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxRaycastLength, _layerMask))
        {
            if (hit.collider.TryGetComponent(out T component))
            {
                RaycastHitPosiion = hit.point;
                Detected?.Invoke(component);
            }
        }
    }
}