using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ResourcesScanner))]
[RequireComponent(typeof(BaseStatisticDisplayer))]
public class Base : MonoBehaviour
{
    [SerializeField] private ResourcesRegistry _resourceRegistry;
    [SerializeField] private List<Unit> _units = new List<Unit>();

    private ResourcesScanner _resourceScanner;
    private BaseStatisticDisplayer _baseStatisticDisplayer;

    private int _resourcesCount;

    private void Awake()
    {
        _resourceScanner = GetComponent<ResourcesScanner>();
        _baseStatisticDisplayer = GetComponent<BaseStatisticDisplayer>();

        _units.ForEach(unit => unit.Initialize(this));
    }

    private void Start()
    {
        _baseStatisticDisplayer.Display(_resourcesCount);
    }

    private void OnEnable()
    {
        _resourceScanner.Detected += OnDetectedResource;
    }

    private void OnDisable()
    {
        _resourceScanner.Detected -= OnDetectedResource;
    }

    public void TakeResource(Resource resource)
    {
        _resourceRegistry.RemoveReservation(resource);
        resource.Collect();
        _resourcesCount++;

        _baseStatisticDisplayer.Display(_resourcesCount);
    }

    private void OnDetectedResource(List<Resource> resources)
    {
        List<Resource> avaliableResources = GetAvaliableResources(resources);
        SendUnitsForResources(avaliableResources);
    }

    private List<Resource> GetAvaliableResources(List<Resource> resources)
    {
        return resources.Where(resource => _resourceRegistry.IsResourceReserved(resource) == false).ToList();
    }

    private void SendUnitsForResources(List<Resource> resources)
    {
        foreach (var resource in resources)
        {
            if (TryGetRandomFreeUnit(out Unit unit))
            {
                unit.GoToResource(resource);
                _resourceRegistry.Reserve(resource);
            }
            else
            {
                Debug.LogWarning("Нет свободных юнитов");
                break;
            }
        }
    }

    private bool TryGetRandomFreeUnit(out Unit unit)
    {
        List<Unit> freeUnits = _units.Where(unit => unit.IsFree).ToList();

        if (freeUnits.Count == 0)
        {
            unit = null;
            return false;
        }

        unit = freeUnits[Random.Range(0, freeUnits.Count)];

        return true;
    }
}