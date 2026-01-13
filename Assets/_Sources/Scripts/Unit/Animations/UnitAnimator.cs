using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnitAnimator : MonoBehaviour
{
    private readonly int Idle = Animator.StringToHash(nameof(Idle));
    private readonly int Run = Animator.StringToHash(nameof(Run));
    private readonly int Carry = Animator.StringToHash(nameof(Carry));

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetIdle()
    {
        _animator.SetTrigger(Idle);
    }

    public void SetRun()
    {
        _animator.SetTrigger(Run);
    }

    public void SetCarry()
    {
        _animator.SetTrigger(Carry);
    }
}