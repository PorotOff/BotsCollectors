using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveToUnitState<T> : IUnitState where T : MonoBehaviour
{
    protected T Target;
    protected NavMeshAgent UnitNavMeshAgent;
    protected UnitAnimator UnitAnimator;

    public event Action<MoveToUnitState<T>> TargetLost;

    public MoveToUnitState(T target, NavMeshAgent unitNavMeshAgent, UnitAnimator unitAnimator)
    {
        Target = target;
        UnitNavMeshAgent = unitNavMeshAgent;
        UnitAnimator = unitAnimator;

        if (Target is ILostable lostable)
        {
            lostable.Lost += OnTargetLost;
        }
    }

    public virtual void Enter()
    {
        UnitNavMeshAgent.isStopped = false;
        UnitNavMeshAgent.SetDestination(Target.transform.position);
        UnitAnimator.SetRun();
    }

    public void Exit() {}

    private void OnTargetLost()
    {
        if (Target is ILostable lostable)
        {
            lostable.Lost -= OnTargetLost;
        }

        TargetLost?.Invoke(this);
    }
}