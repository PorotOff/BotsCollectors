using System;

public class ResourcesAccounter
{
    public event Action ChangedCount;

    public int Count { get; private set; } = 0;

    public void Add(int count)
    {
        if (count <= 0)
            return;
        
        Count += count;

        ChangedCount?.Invoke();
    }

    public void Remove(int desiredCount)
    {
        if (desiredCount <= 0)
            return;

        if (HasEnoughResources(desiredCount) == false)
            return;
        
        Count -= desiredCount;

        ChangedCount?.Invoke();
    }

    public bool HasEnoughResources(int desiredCount)
    {
        return Count >= desiredCount;
    }
}