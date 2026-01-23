using System.Collections;
using UnityEngine;

public class CyclicResourcesSpawner : Spawner<Resource>
{
    [Header("Spawner settings")]
    [SerializeField, Min(0f)] private float _minSpawnTimeSeconds;
    [SerializeField, Min(0f)] private float _maxSpawnTimeSeconds;
    [Header("Spawning settings")]
    [SerializeField] private Transform _minSpawnBoundingBoxVertex;
    [SerializeField] private Transform _maxSpawnBoundingBoxVertex;
    [SerializeField, Min(0)] private float _withinSpawnBoundigBoxCheckRadius;

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

            Vector3 randomPositionWithinSpawnBoundingBox;

            do
            {
                float randomXPosition = Random.Range(_minSpawnBoundingBoxVertex.position.x, _maxSpawnBoundingBoxVertex.position.x);
                float randomZPosition = Random.Range(_minSpawnBoundingBoxVertex.position.z, _maxSpawnBoundingBoxVertex.position.z);
                randomPositionWithinSpawnBoundingBox = new Vector3(randomXPosition, _minSpawnBoundingBoxVertex.position.y, randomZPosition);
            }
            while (NavMeshUtils.IsPositionOnNavMesh(randomPositionWithinSpawnBoundingBox, _withinSpawnBoundigBoxCheckRadius) == false);

            Resource resource = Spawn();
            resource.transform.position = randomPositionWithinSpawnBoundingBox;
        }
    }
}