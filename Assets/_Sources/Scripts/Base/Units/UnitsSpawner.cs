using System.Collections.Generic;
using UnityEngine;

public class UnitsSpawner : Spawner<Unit>
{
    [SerializeField] private Transform _minSpawnpoint;
    [SerializeField] private Transform _maxSpawnpoint;
    [SerializeField, Min(0)] private float _maxDistanceToNavMesh = 2;

    public Unit Spawn(Base @base)
    {
        float randomXPosition = Random.Range(_minSpawnpoint.position.x, _maxSpawnpoint.position.x);
        float randomZPosition = Random.Range(_minSpawnpoint.position.z, _maxSpawnpoint.position.z);
        Vector3 randomPosition = new Vector3(randomXPosition, _minSpawnpoint.position.y, randomZPosition);
        Vector3 positionOnNavMesh;

        while (NavMeshUtils.TryGetPositionOnSurface(randomPosition, _maxDistanceToNavMesh, out positionOnNavMesh) == false) { }

        Unit unit = Spawn();

        unit.Initialize(@base, positionOnNavMesh);

        float minYRotation = 0f;
        float maxYRotation = 360f;
        unit.transform.rotation = Quaternion.Euler(0f, Random.Range(minYRotation, maxYRotation), 0f);

        return unit;
    }

    public List<Unit> Spawn(Base @base, int count)
    {
        List<Unit> units = new List<Unit>();

        for (int i = 0; i < count; i++)
        {
            Unit unit = Spawn(@base);
            units.Add(unit);
        }

        return units;
    }
}