using UnityEngine;

public class PrefabInstancesContainer<T> : MonoBehaviour where T : MonoBehaviour, IPooledObject<T>
{
    
}