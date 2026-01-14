using System.Collections.Generic;
using UnityEngine;

public class ResourcesRegistry : MonoBehaviour
{
    private List<Resource> _reservedResources = new List<Resource>();

    public void Reserve(Resource resource)
    {
        _reservedResources.Add(resource);
    }

    public void RemoveReservation(Resource resource)
    {
        _reservedResources.Remove(resource);
    }

    public bool IsResourceReserved(Resource resource)
    {
        return _reservedResources.Contains(resource);
    }
}