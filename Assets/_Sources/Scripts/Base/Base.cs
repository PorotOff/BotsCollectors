using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourcesScanner))]
[RequireComponent(typeof(UnitsSpawner))]
[RequireComponent(typeof(UnitsRegistry))]
[RequireComponent(typeof(UnitSendingTimer))]
[RequireComponent(typeof(BaseResourcesStatisticDisplayer))]
public class Base : MonoBehaviour, IPooledObject<Base>, IKiller
{
    [SerializeField] private int _maxUnitsCount = 5;
    [SerializeField] private int _minUnitsCount = 1;
    [SerializeField, Min(0)] private int _newUnitCostResources = 3;
    [SerializeField, Min(0)] private int _newBaseCostResources = 5;

    private ResourcesScanner _resourceScanner;
    private UnitsSpawner _unitsSpawner;
    private UnitsRegistry _unitsRegistry;
    private UnitSendingTimer _unitSendingTimer;
    private BaseResourcesStatisticDisplayer _baseStatisticDisplayer;

    private ResourcesAccounter _resourcesAccounter;

    private ResourcesRegistry _resourcesRegistry;

    public event Action<Base> Released;
    public event Action<Unit, Flag> PickedUpUnitFlag;

    public Flag Flag { get; private set; }

    private void OnDisable()
    {
        _resourceScanner.Detected -= OnDetectedResources;
        _resourcesAccounter.ChangedCount -= OnResourcesCountChanged;
        _unitSendingTimer.Ticked -= SendUnitBuildBase;
    }

    private void Start()
    {
        _baseStatisticDisplayer.Display(_resourcesAccounter.Count);
    }

    public void Initialize(Transform unitsContainer, ResourcesRegistry resourcesRegistry, int initialUnitsCount)
    {
        Initialize();

        _unitsSpawner.Initialize(unitsContainer);
        _resourcesRegistry = resourcesRegistry;
        List<Unit> initialUnits = _unitsSpawner.Spawn(this, initialUnitsCount);

        _unitsRegistry.Add(initialUnits);
    }

    public void Initialize(Transform unitsContainer, ResourcesRegistry resourcesRegistry, Unit initialUnit)
    {
        Initialize();

        _unitsSpawner.Initialize(unitsContainer);
        _resourcesRegistry = resourcesRegistry;
        _unitsRegistry.Add(initialUnit);
    }

    public void Initialize()
    {
        _resourceScanner = GetComponent<ResourcesScanner>();
        _unitsSpawner = GetComponent<UnitsSpawner>();
        _unitsRegistry = GetComponent<UnitsRegistry>();
        _unitSendingTimer = GetComponent<UnitSendingTimer>();
        _baseStatisticDisplayer = GetComponent<BaseResourcesStatisticDisplayer>();

        _resourcesAccounter = new ResourcesAccounter();

        _resourceScanner.Detected += OnDetectedResources;
        _resourcesAccounter.ChangedCount += OnResourcesCountChanged;
        _unitSendingTimer.Ticked += SendUnitBuildBase;
    }

    public void Release()
    {
        Released?.Invoke(this);
    }

    public void TakeResource(Resource resource)
    {
        _resourcesRegistry.Remove(resource);
        resource.Collect();
        _resourcesAccounter.Add(1);
    }

    public void SetFlag(Flag flag)
    {
        Flag = flag;
        _unitSendingTimer.StartSending();
    }

    public bool HasFlag()
    {
        return Flag != null;
    }

    private void SendUnitsForResources(List<Resource> resources)
    {
        foreach (var resource in resources)
        {
            if (_unitsRegistry.TryGetRandomFreeUnit(out Unit unit))
            {
                unit.GoToResource(resource);
                _resourcesRegistry.Add(resource);
            }
            else
            {
                Debug.LogWarning($"{name}: Нет свободных юнитов, чтобы отправить их за ресурсами");
                break;
            }
        }
    }

    private void OnDetectedResources(List<Resource> resources)
    {
        List<Resource> avaliableResources = _resourcesRegistry.GetNotRegistered(resources);
        SendUnitsForResources(avaliableResources);
    }

    private void OnResourcesCountChanged()
    {
        SpawnUnit();
        _baseStatisticDisplayer.Display(_resourcesAccounter.Count);
    }

    private void SpawnUnit()
    {
        if (_unitsRegistry.Count >= _maxUnitsCount)
            return;

        if (HasFlag() && _unitsRegistry.Count > _minUnitsCount)
            return;

        if (_resourcesAccounter.HasEnoughResources(_newUnitCostResources) == false)
            return;

        Unit newUnit = _unitsSpawner.Spawn(this);
        _unitsRegistry.Add(newUnit);

        _resourcesAccounter.Remove(_newUnitCostResources);
    }

    private void SendUnitBuildBase()
    {
        if (_unitsRegistry.Count <= _minUnitsCount)
            return;

        if (HasFlag() == false)
            return;

        if (_unitsRegistry.HasBusyGoingFlagUnit())
            return;

        if (_resourcesAccounter.HasEnoughResources(_newBaseCostResources) == false)
            return;

        if (_unitsRegistry.TryGetRandomFreeUnit(out Unit unit) == false)
        {
            Debug.LogWarning($"{name}: Нет свободных юнитов, чтобы отправить их строить базу");
            return;
        }

        _resourcesAccounter.Remove(_newBaseCostResources);
        unit.GoToFlag(Flag);

        unit.PickedUpFlag += OnPickedUpUnitFlag;
    }

    private void OnPickedUpUnitFlag(Unit unit)
    {
        unit.PickedUpFlag -= OnPickedUpUnitFlag;

        PickedUpUnitFlag?.Invoke(unit, Flag);

        _unitsRegistry.Remove(unit);
        Flag = null;

        _unitSendingTimer.StopSending();
    }
}