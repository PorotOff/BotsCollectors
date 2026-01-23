using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtils
{
    public static bool IsPositionOnNavMesh(Vector3 position, float checkRadius)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit _, checkRadius, NavMesh.AllAreas))
        {
            return true;
        }

        return false;
    }
}