using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtils
{
    public static bool TryGetPositionOnSurface(Vector3 sourcePosition, float maxDistance, out Vector3 positionOnNavMesh)
    {
        if (NavMesh.SamplePosition(sourcePosition, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            positionOnNavMesh = hit.position;
            return true;
        }
        
        positionOnNavMesh = default;

        return false;
    }
}