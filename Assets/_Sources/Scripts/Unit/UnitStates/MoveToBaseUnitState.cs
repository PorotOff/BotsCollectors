using UnityEngine.AI;

public class MoveToBaseUnitState : MoveToUnitState<Base>
{
    public MoveToBaseUnitState(Base target, NavMeshAgent unitNavMeshAgent, UnitAnimator unitAnimator) : base(target, unitNavMeshAgent, unitAnimator) { }
}