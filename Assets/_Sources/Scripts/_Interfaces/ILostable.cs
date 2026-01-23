using System;

public interface ILostable
{
    public event Action Lost;
}