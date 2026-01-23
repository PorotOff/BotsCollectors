using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private bool _isFlip;

    private Vector3 _horizontalFlipDirection = new Vector3(0f, 180f, 0f);

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(_camera.transform);

        if (_isFlip)
        {
            transform.Rotate(_horizontalFlipDirection);
        }
    }
}