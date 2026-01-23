using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshRebuilder : MonoBehaviour
{
    private NavMeshSurface _navMeshSurface;

    private void Awake()
    {
        _navMeshSurface = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        _navMeshSurface.BuildNavMesh();
    }
}