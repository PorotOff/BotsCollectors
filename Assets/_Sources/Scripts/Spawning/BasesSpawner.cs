using System;
using System.Collections.Generic;
using UnityEngine;

public class BasesSpawner : Spawner<Base>
{
    [SerializeField] private ResourcesRegistry _resourcesRegistry;
    [SerializeField] private Base _initialBase;
    [SerializeField] private int _initialUnitsCount;
    [SerializeField] private Transform _unitsContainer;

    private List<Base> _bases = new List<Base>();

    public event Action SpawnedBase;

    private void Start()
    {
        _initialBase.Initialize(_unitsContainer, _resourcesRegistry, _initialUnitsCount);

        _initialBase.PickedUpUnitFlag += OnPickedUpUnitFlag;

        _bases.Add(_initialBase);
    }

    private void OnPickedUpUnitFlag(Unit unit, Flag flag)
    {
        Base @base = Spawn(unit, flag);        
        @base.PickedUpUnitFlag += OnPickedUpUnitFlag;
        _bases.Add(@base);

        unit.Initialize(@base);

        SpawnedBase?.Invoke();
    }

    protected override void OnPooledObjectReleased(Base pooledObject)
    {
        pooledObject.PickedUpUnitFlag -= OnPickedUpUnitFlag;
        base.OnPooledObjectReleased(pooledObject);
    }

    private Base Spawn(Unit unit, Flag flag)
    {
        Base @base = Spawn();

        Vector3 position = flag.transform.position;
        position.y = @base.transform.position.y;

        Quaternion rotation = UnityEngine.Random.rotation;
        rotation.x = 0f;
        rotation.z = 0f;

        @base.transform.position = position;
        @base.transform.rotation = rotation;
        @base.Initialize(_unitsContainer, _resourcesRegistry, unit);

        return @base;
    }
}