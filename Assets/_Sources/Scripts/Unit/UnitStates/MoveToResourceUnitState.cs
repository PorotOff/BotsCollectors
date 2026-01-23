using UnityEngine.AI;

public class MoveToResourceUnitState : MoveToUnitState<Resource>
{
    public MoveToResourceUnitState(Resource target, NavMeshAgent unitNavMeshAgent, UnitAnimator unitAnimator) : base(target, unitNavMeshAgent, unitAnimator) { }
}