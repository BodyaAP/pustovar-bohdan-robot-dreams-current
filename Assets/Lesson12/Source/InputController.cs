using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static event Action<Vector2> OnMoveInput;
    public static event Action<Vector2> OnLookInput;
    public static event Action<bool> OnFireInput;
    public static event Action OnSphereCastInput;

    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private string _mapName;
    [SerializeField] private string _moveName;
    [SerializeField] private string _lookName;
    [SerializeField] private string _fireName;
    [SerializeField] private string _sphereCastName;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _fireAction;
    private InputAction _sphereCastAction;

    private bool _inputUpdated;

    private void OnEnable()
    {
        _inputActionAsset.Enable();
        InputActionMap actionMap = _inputActionAsset.FindActionMap(_mapName);
        _moveAction = actionMap[_moveName];
        _lookAction = actionMap[_lookName];
        _fireAction = actionMap[_fireName];
        _sphereCastAction = actionMap[_sphereCastName];

        _moveAction.performed += MovePerformedHandler;
        _moveAction.canceled += MoveCancledHandler;

        _lookAction.performed += LookPerformedHandler;
        _lookAction.canceled += LookCanceledHandler;

        _fireAction.performed += FirePerformedHandler;
        _fireAction.canceled += FireCanceledHandler;

        _sphereCastAction.performed += SphereCastPerfomedHandler;
    }

    private void OnDisable()
    {
        _inputActionAsset?.Disable();
    }

    private void OnDestroy()
    {
        OnMoveInput = null;
        OnLookInput = null;
        OnFireInput = null;
        OnSphereCastInput = null;
    }

    private void MovePerformedHandler(InputAction.CallbackContext context)
    {
        OnMoveInput?.Invoke(context.ReadValue<Vector2>());
    }

    private void MoveCancledHandler(InputAction.CallbackContext context)
    {
        OnMoveInput?.Invoke(context.ReadValue<Vector2>());
    }

    private void LookPerformedHandler(InputAction.CallbackContext context)
    {
        OnLookInput?.Invoke(context.ReadValue<Vector2>());
    }

    private void LookCanceledHandler(InputAction.CallbackContext context)
    {
        OnLookInput?.Invoke(context.ReadValue<Vector2>());
    }

    private void FirePerformedHandler(InputAction.CallbackContext context)
    {
        OnFireInput?.Invoke(true);
    }

    private void FireCanceledHandler(InputAction.CallbackContext context)
    {
        OnFireInput?.Invoke(false);
    }

    private void SphereCastPerfomedHandler(InputAction.CallbackContext context)
    {
        OnSphereCastInput?.Invoke();
    }
}
