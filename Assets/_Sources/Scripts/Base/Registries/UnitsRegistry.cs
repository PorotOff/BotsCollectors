using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsRegistry : Registry<Unit>
{
    public bool TryGetRandomFreeUnit(out Unit unit)
    {
        List<Unit> freeUnits = RegisteredObjects.Where(unit => unit.IsFree).ToList();

        if (freeUnits.Count == 0)
        {
            unit = null;
            return false;
        }

        unit = freeUnits[Random.Range(0, freeUnits.Count)];

        return true;
    }

    public bool HasBusyGoingFlagUnit()
    {
        Unit busyGoingFlagUnit = RegisteredObjects.FirstOrDefault(unit => unit.IsBusyGoingFlag);

        return busyGoingFlagUnit != null;
    }
}