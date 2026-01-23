using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : MonoBehaviour
{
    private InputActions _inputActions;

    public event Action<Vector2> Pressed;

    private void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Game.Enable();
    }

    private void OnEnable()
    {
        _inputActions.Game.Press.performed += OnPressed;
    }

    private void OnDisable()
    {
        _inputActions.Game.Press.performed -= OnPressed;
    }

    private void OnPressed(InputAction.CallbackContext context)
    {
        Vector2 pressPosition = _inputActions.Game.PressPosition.ReadValue<Vector2>();
        Pressed?.Invoke(pressPosition);
    }
}