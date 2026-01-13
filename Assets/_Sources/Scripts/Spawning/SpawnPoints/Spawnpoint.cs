using UnityEngine;

public class Spawnpoint<T> : MonoBehaviour where T : MonoBehaviour, IPooledObject<T>
{
    protected T Spawnable;

    public bool IsFree => Spawnable == null;

    public virtual void Occupy(T spawnable)
    {
        Spawnable = spawnable;
    }

    public virtual void MakeFree()
    {
        Spawnable = null;
    }
}