using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourcesScanner))]
[RequireComponent(typeof(UnitsSpawner))]
public class Base : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField, Min(0)] private int _initialUnitsCount = 3;
    [Header("Expansion settings")]
    [SerializeField, Min(0)] private int _newUnitCostResources = 3;
    [SerializeField, Min(0)] private int _newBaseCostResources = 5;

    private ResourcesScanner _resourceScanner;
    private UnitsSpawner _unitCreator;

    private UnitsRegistry _unitsRegistry;
    private ResourcesAccounter _resourcesAccounter;

    private ResourcesRegistry _resourcesRegistry;

    public event Action ChangedResourcesCount;

    private void Awake()
    {
        _resourceScanner = GetComponent<ResourcesScanner>();
        _unitCreator = GetComponent<UnitsSpawner>();

        _unitsRegistry = new UnitsRegistry();
        _resourcesAccounter = new ResourcesAccounter();

        _resourcesRegistry = FindFirstObjectByType<ResourcesRegistry>(FindObjectsInactive.Include);

        if (_resourcesRegistry == null)
        {
            Debug.LogError($"На сцене нет {nameof(_resourcesRegistry)}");
        }
    }

    private void Start()
    {
        List<Unit> initialUnits = _unitCreator.Spawn(this, _initialUnitsCount);
        _unitsRegistry.Add(initialUnits);
    }

    private void OnEnable()
    {
        _resourceScanner.Detected += OnDetectedResource;

        _resourcesAccounter.ChangedCount += OnChangedResourcesCount;
        _resourcesAccounter.ChangedCount += OnCollectedEnoughtResourcesForCreateUnit;
    }

    private void OnDisable()
    {
        _resourceScanner.Detected -= OnDetectedResource;

        _resourcesAccounter.ChangedCount -= OnChangedResourcesCount;
        _resourcesAccounter.ChangedCount -= OnCollectedEnoughtResourcesForCreateUnit;
    }

    public void TakeResource(Resource resource)
    {
        _resourcesRegistry.RemoveReservation(resource);
        resource.Collect();
        _resourcesAccounter.Add(1);
    }

    private void OnDetectedResource(List<Resource> resources)
    {
        List<Resource> avaliableResources = _resourcesRegistry.GetAvaliableResources(resources);
        SendUnitsForResources(avaliableResources);
    }

    private void OnChangedResourcesCount()
    {
        ChangedResourcesCount?.Invoke();
    }

    private void OnCollectedEnoughtResourcesForCreateUnit()
    {
        if (_resourcesAccounter.HasEnoughResources(_newUnitCostResources))
        {
            Unit newUnit = _unitCreator.Spawn(this);
            _unitsRegistry.Add(newUnit);

            _resourcesAccounter.Remove(_newBaseCostResources);
        }
    }

    private void SendUnitsForResources(List<Resource> resources)
    {
        foreach (var resource in resources)
        {
            if (_unitsRegistry.TryGetRandomFreeUnit(out Unit unit))
            {
                unit.GoToResource(resource);
                _resourcesRegistry.Reserve(resource);
            }
            else
            {
                Debug.LogWarning("Нет свободных юнитов");
                break;
            }
        }
    }
}