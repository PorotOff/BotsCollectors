using UnityEngine.AI;

public class IdleUnitState : IUnitState
{
    private NavMeshAgent _unitNavMeshAgent;
    private UnitAnimator _unitAnimator;

    public IdleUnitState(NavMeshAgent unitNavMeshAgent, UnitAnimator unitAnimator)
    {
        _unitNavMeshAgent = unitNavMeshAgent;
        _unitAnimator = unitAnimator;
    }

    public void Enter()
    {
        _unitNavMeshAgent.isStopped = true;
        _unitAnimator.SetIdle();
    }

    public void Exit() { }
}