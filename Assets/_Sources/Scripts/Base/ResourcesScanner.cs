using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesScanner : MonoBehaviour
{
    [SerializeField, Min(0)] private float _scanIntervalSeconds = 3f;
    [SerializeField, Min(0)] private float _scanRadius = 100f;
    [Header("For debug")]
    [SerializeField] private bool _isDrawScanRadius;

    private Coroutine _coroutine;

    public event Action<List<Resource>> Detected;

    private void OnEnable()
    {
        StartScan();
    }

    private void OnDisable()
    {
        StopScan();
    }

    private void StartScan()
    {
        StopScan();
        _coroutine = StartCoroutine(Scan());
    }

    private void StopScan()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator Scan()
    {
        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(_scanIntervalSeconds);

        while (enabled)
        {
            yield return wait;

            List<Resource> foundResources = new List<Resource>();
            List<Collider> hits = Physics.OverlapSphere(transform.position, _scanRadius).ToList();

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Resource resource))
                {
                    foundResources.Add(resource);
                }
            }

            if (foundResources.Count > 0)
            {
                Detected?.Invoke(foundResources);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_isDrawScanRadius)
        {
            Gizmos.DrawSphere(transform.position, _scanRadius);
        }
    }
}