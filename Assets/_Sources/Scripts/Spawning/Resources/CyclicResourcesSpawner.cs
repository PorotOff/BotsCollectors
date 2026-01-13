using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class CyclicResourcesSpawner : Spawner<Resource>
{
    [SerializeField] private List<ResourceSpawnpoint> _spawnpoints;
    [SerializeField] private NavMeshSurface _navMeshSurface;
    [SerializeField, Min(0f)] private float _minSpawnTimeSeconds;
    [SerializeField, Min(0f)] private float _maxSpawnTimeSeconds;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        StartSpawn();
    }

    private void OnDisable()
    {
        StopSpawn();
    }

    private void StartSpawn()
    {
        StopSpawn();
        _coroutine = StartCoroutine(SpawnCyclic());
    }

    private void StopSpawn()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator SpawnCyclic()
    {
        while (enabled)
        {
            float randomSpawnTimeSeconds = Random.Range(_minSpawnTimeSeconds, _maxSpawnTimeSeconds);
            yield return new WaitForSecondsRealtime(randomSpawnTimeSeconds);

            if (TryGetRandomSpawnPosition(out ResourceSpawnpoint resourceSpawnpoint))
            {
                Resource resource = Spawn();
                resource.transform.position = resourceSpawnpoint.transform.position;

                resourceSpawnpoint.Occupy(resource);
            }
            else
            {
                Debug.LogWarning("Свободных точек спавна нет");
            }
        }
    }

    private bool TryGetRandomSpawnPosition(out ResourceSpawnpoint resourceSpawnpoint)
    {
        List<ResourceSpawnpoint> ableSpawnpoints = new List<ResourceSpawnpoint>(_spawnpoints);

        while (ableSpawnpoints.Count > 0)
        {
            resourceSpawnpoint = GetRandomSpawnpoint(ableSpawnpoints);
            ableSpawnpoints.Remove(resourceSpawnpoint);

            if (resourceSpawnpoint.IsFree)
            {
                return true;
            }
        }

        resourceSpawnpoint = null;

        return false;
    }

    private ResourceSpawnpoint GetRandomSpawnpoint(List<ResourceSpawnpoint> spawnpoints)
    {
        int randomIndex = Random.Range(0, spawnpoints.Count);
        return spawnpoints[randomIndex];
    }
}