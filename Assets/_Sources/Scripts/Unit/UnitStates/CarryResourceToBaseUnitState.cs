using UnityEngine.AI;

public class CarryResourceToBaseUnitState : MoveToUnitState<Base>
{
    public CarryResourceToBaseUnitState(Base target, NavMeshAgent unitNavMeshAgent, UnitAnimator unitAnimator) : base(target, unitNavMeshAgent, unitAnimator) { }

    public override void Enter()
    {
        UnitNavMeshAgent.SetDestination(Target.transform.position);
        UnitAnimator.SetCarry();
    }
}