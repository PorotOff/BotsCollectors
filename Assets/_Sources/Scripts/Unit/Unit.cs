using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(UnitAnimator))]
[RequireComponent(typeof(ResourceComponentTriggerDetector))]
[RequireComponent(typeof(BaseComponentTriggerDetector))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _angularSpeed = 500f;
    [SerializeField] private Transform _carryingPoint;

    private NavMeshAgent _navMeshAgent;
    private UnitAnimator _unitAnimator;
    private ResourceComponentTriggerDetector _resourceDetector;
    private BaseComponentTriggerDetector _baseComponentTriggerDetector;

    private Base _base;
    private Resource _targetResource;
    private Resource _resource;
    [SerializeField] private bool _isAtBase;

    public bool IsFree => _targetResource == null && _resource == null;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _unitAnimator = GetComponent<UnitAnimator>();
        _resourceDetector = GetComponent<ResourceComponentTriggerDetector>();
        _baseComponentTriggerDetector = GetComponent<BaseComponentTriggerDetector>();

        _navMeshAgent.speed = _speed;
        _navMeshAgent.acceleration = _acceleration;
        _navMeshAgent.angularSpeed = _angularSpeed;
    }

    private void OnEnable()
    {
        _resourceDetector.Detected += OnResourceDetected;

        _baseComponentTriggerDetector.Detected += OnBaseDetected;
        _baseComponentTriggerDetector.Lost += OnBaseLost;
    }

    private void OnDisable()
    {
        _resourceDetector.Detected -= OnResourceDetected;

        _baseComponentTriggerDetector.Detected -= OnBaseDetected;
        _baseComponentTriggerDetector.Lost -= OnBaseLost;
    }

    public void Initialize(Base @base)
    {
        _base = @base;
    }

    public void GoToResource(Resource resource)
    {
        _targetResource = resource;

        _targetResource.PickedUp += OnTargetResourcePickedUp;

        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_targetResource.transform.position);

        _unitAnimator.SetRun();
    }

    private void OnResourceDetected(Resource resource)
    {
        if (resource != _targetResource)
        {
            return;
        }

        _resource = resource;
        _resource.Pickup();

        _resource.transform.SetParent(_carryingPoint);
        _resource.transform.localPosition = Vector3.zero;

        _unitAnimator.SetCarry();
    }

    private void OnTargetResourcePickedUp()
    {
        _targetResource.PickedUp -= OnTargetResourcePickedUp;
        _targetResource.GetComponent<Collider>().enabled = false;
        _targetResource = null;

        if (_base != null)
        {
            _navMeshAgent.SetDestination(_base.transform.position);
        }
        else
        {
            Debug.LogError($"Юнит {name} безбазный. Ему некуда идти");
        }
    }

    private void OnBaseDetected(Base @base)
    {
        if (_isAtBase)
        {
            return;
        }

        if (_resource != null)
        {
            @base.TakeResource(_resource);
            _resource = null;
        }

        _isAtBase = true;
        _navMeshAgent.isStopped = true;

        _unitAnimator.SetIdle();
    }

    private void OnBaseLost()
    {
        _isAtBase = false;
    }

    // todo Реализовать нормальное выключение коллизии у ресурса
    // todo Исправить вывод предупреждения об отсутствии юнитов (чтобы один раз было за все ресурсы, а не за каждый ресурс по выводу)
}