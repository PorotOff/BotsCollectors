using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsRegistry
{
    private List<Unit> _units = new List<Unit>();

    public void Add(List<Unit> units)
    {
        _units.AddRange(units);
    }

    public void Add(Unit unit)
    {
        _units.Add(unit);
    }

    public void Remove(List<Unit> units)
    {
        foreach (var unit in units)
        {
            if (HasUnit(unit))
            {
                _units.Remove(unit);
            }
        }
    }

    public void Remove(Unit unit)
    {
        if (HasUnit(unit))
        {
            _units.Add(unit);
        }
    }

    public bool HasUnit(Unit unit)
    {
        return _units.Contains(unit);
    }

    public bool TryGetRandomFreeUnit(out Unit unit)
    {
        List<Unit> freeUnits = _units.Where(unit => unit.IsFree).ToList();

        if (freeUnits.Count == 0)
        {
            unit = null;
            return false;
        }

        unit = freeUnits[Random.Range(0, freeUnits.Count)];

        return true;
    }
}