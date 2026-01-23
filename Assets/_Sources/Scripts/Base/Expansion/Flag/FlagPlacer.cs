using UnityEngine;

[RequireComponent(typeof(FlagsSpawner))]
[RequireComponent(typeof(BaseRaycastComponentDetector))]
[RequireComponent(typeof(GameFieldRaycastComponentDetector))]
public class FlagPlacer : MonoBehaviour
{
    private FlagsSpawner _flagsSpawner;
    private BaseRaycastComponentDetector _baseRaycastComponentDetector;
    private GameFieldRaycastComponentDetector _gameFieldRaycastComponentDetector;

    private Base _selectedBase;

    private void Awake()
    {
        _flagsSpawner = GetComponent<FlagsSpawner>();
        _baseRaycastComponentDetector = GetComponent<BaseRaycastComponentDetector>();
        _gameFieldRaycastComponentDetector = GetComponent<GameFieldRaycastComponentDetector>();
    }

    private void OnEnable()
    {
        _baseRaycastComponentDetector.Detected += SetBase;
        _gameFieldRaycastComponentDetector.Detected += SetFlag;
    }

    private void OnDisable()
    {
        _baseRaycastComponentDetector.Detected -= SetBase;
        _gameFieldRaycastComponentDetector.Detected -= SetFlag;
    }

    private void SetBase(Base @base)
    {
        _selectedBase = @base;
    }

    private void SetFlag(GameField gameField)
    {
        if (_selectedBase == null)
            return;

        Vector3 setPosition = _gameFieldRaycastComponentDetector.RaycastHitPosiion;

        if (_selectedBase.HasFlag())
        {
            _selectedBase.Flag.PlaceAtPosition(setPosition);
        }
        else
        {
            Flag flag = _flagsSpawner.Spawn();

            flag.PlaceAtPosition(setPosition);
            _selectedBase.SetFlag(flag);
        }

        _selectedBase = null;
    }
}