using UnityEngine;

[RequireComponent(typeof(NavMeshRebuilder))]
public class GameField : MonoBehaviour
{
    [SerializeField] private BasesSpawner _basesSpawner;

    private NavMeshRebuilder _navMeshRebuilder;

    private void Awake()
    {
        _navMeshRebuilder = GetComponent<NavMeshRebuilder>();
    }

    private void OnEnable()
    {
        _basesSpawner.SpawnedBase += OnSpawnedBase;
    }

    private void OnDisable()
    {
        _basesSpawner.SpawnedBase -= OnSpawnedBase;
    }

    private void OnSpawnedBase()
    {
        _navMeshRebuilder.Rebuild();
    }
}