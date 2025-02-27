using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _speed = 4.8f;

    private Transform _transform;
    private Vector3 _localDirection;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        InputController.OnMoveInput += MoveHandler;
    }

    private void FixedUpdate()
    {
        Vector3 forward = _transform.forward;
        Vector3 right = _transform.right;
        Vector3 movement = forward * _localDirection.z + right * _localDirection.x;
        _characterController.SimpleMove(movement * _speed);
    }

    private void MoveHandler(Vector2 moveInput)
    {
        _localDirection = new Vector3(moveInput.x, 0, moveInput.y);
    }
}
