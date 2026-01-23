using UnityEngine.AI;

public class MoveToFlagUnitState : MoveToUnitState<Flag>
{
    public MoveToFlagUnitState(Flag target, NavMeshAgent unitNavMeshAgent, UnitAnimator unitAnimator) : base(target, unitNavMeshAgent, unitAnimator)
    {
        Target.Replaced += OnFlagReplaced;
        Target.Released += OnFlagReleased;
    }

    private void OnFlagReplaced()
    {
        Enter();
    }

    private void OnFlagReleased(Flag flag)
    {
        Target.Replaced -= OnFlagReplaced;
        Target.Released -= OnFlagReleased;
    }
}