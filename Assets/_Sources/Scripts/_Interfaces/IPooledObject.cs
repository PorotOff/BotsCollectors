using System;

public interface IPooledObject<T>
{
    public event Action<T> Released;

    public void Release();
}