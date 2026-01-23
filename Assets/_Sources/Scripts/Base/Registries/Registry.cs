using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Registry<T> : MonoBehaviour
{
    protected List<T> RegisteredObjects = new List<T>();

    public int Count => RegisteredObjects.Count;

    public void Add(List<T> Objects)
    {
        RegisteredObjects.AddRange(Objects);
    }

    public void Add(T @object)
    {
        RegisteredObjects.Add(@object);
    }

    public void Remove(T @object)
    {
        RegisteredObjects.Remove(@object);
    }

    public bool HasObject(T @object)
    {
        return RegisteredObjects.Contains(@object);
    }

    public List<T> GetNotRegistered(List<T> objects)
    {
        return objects.Where(@object => HasObject(@object) == false).ToList();
    }
}