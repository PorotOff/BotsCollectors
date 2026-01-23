using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(UnitAnimator))]
[RequireComponent(typeof(ResourceTargetTriggerDetector))]
[RequireComponent(typeof(BaseTargetTriggerDetector))]
[RequireComponent(typeof(FlagTargetTriggerDetector))]
public class Unit : MonoBehaviour, IPooledObject<Unit>
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _angularSpeed = 500f;
    [SerializeField] private Transform _carryingPoint;

    private NavMeshAgent _navMeshAgent;
    private UnitAnimator _animator;
    private ResourceTargetTriggerDetector _resourceDetector;
    private BaseTargetTriggerDetector _baseDetector;
    private FlagTargetTriggerDetector _flagDetector;

    private Base _base;
    private Resource _resource;
    private IUnitState _unitState;
    private bool _isAtBase = true;

    public event Action<Unit> Released;
    public event Action<Unit> PickedUpFlag;

    public bool IsFree => _isAtBase;
    public bool IsBusyGoingFlag => _unitState is MoveToFlagUnitState;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<UnitAnimator>();
        _resourceDetector = GetComponent<ResourceTargetTriggerDetector>();
        _baseDetector = GetComponent<BaseTargetTriggerDetector>();
        _flagDetector = GetComponent<FlagTargetTriggerDetector>();

        _navMeshAgent.speed = _speed;
        _navMeshAgent.acceleration = _acceleration;
        _navMeshAgent.angularSpeed = _angularSpeed;
    }

    private void OnEnable()
    {
        _resourceDetector.Detected += OnTargetResourceDetected;
        _baseDetector.Detected += OnBaseDetected;
        _baseDetector.Lost += OnBaseLost;
        _flagDetector.Detected += OnFlagDetected;
    }

    private void OnDisable()
    {
        _resourceDetector.Detected -= OnTargetResourceDetected;
        _baseDetector.Detected -= OnBaseDetected;
        _baseDetector.Lost -= OnBaseLost;
        _flagDetector.Detected -= OnFlagDetected;
    }

    public void Initialize(Base @base)
    {
        _base = @base;
        _baseDetector.Initialize(_base);
    }

    public void Release()
    {
        Released?.Invoke(this);
    }

    public void GoToResource(Resource resource)
    {
        _resourceDetector.Initialize(resource);

        MoveToResourceUnitState moveState = new MoveToResourceUnitState(resource, _navMeshAgent, _animator);
        moveState.TargetLost += OnTargetResourceLost;
        SetState(moveState);
    }

    public void GoToFlag(Flag flag)
    {
        _flagDetector.Initialize(flag);

        MoveToFlagUnitState moveState = new MoveToFlagUnitState(flag, _navMeshAgent, _animator);
        moveState.TargetLost += OnFlagLost;
        SetState(moveState);
    }

    private void SetState(IUnitState unitState)
    {
        if (_unitState != null)
        {
            _unitState.Exit();
        }

        _unitState = unitState;
        _unitState.Enter();
    }

    private void OnTargetResourceDetected(Resource resource)
    {
        if (_base == null)
        {
            Debug.LogError($"Юнит {name} безбазный. Ему некуда идти");
            return;
        }

        _resource = resource;
        _resource.PickUp(_carryingPoint);

        SetState(new CarryResourceToBaseUnitState(_base, _navMeshAgent, _animator));
    }

    private void OnTargetResourceLost(MoveToUnitState<Resource> moveToResourceUnitState)
    {
        moveToResourceUnitState.TargetLost -= OnTargetResourceLost;

        if (_isAtBase)
            return;
        
        SetState(new MoveToBaseUnitState(_base, _navMeshAgent, _animator));
    }

    private void OnFlagDetected(Flag flag)
    {
        if (IsBusyGoingFlag == false)
            return;

        flag.PickUp();
        _flagDetector.Deinitialize();

        PickedUpFlag?.Invoke(this);
    }

    private void OnFlagLost(MoveToUnitState<Flag> moveToFlagUnitState)
    {
        moveToFlagUnitState.TargetLost -= OnFlagLost;
        
        if (_isAtBase)
            return;

        SetState(new MoveToBaseUnitState(_base, _navMeshAgent, _animator));
    }

    private void OnBaseDetected(Base @base)
    {
        if (_isAtBase)
            return;

        _isAtBase = true;
        SetState(new IdleUnitState(_navMeshAgent, _animator));

        if (_resource == null)
            return;

        @base.TakeResource(_resource);
        _resource = null;
    }

    private void OnBaseLost()
    {
        _isAtBase = false;
    }
}