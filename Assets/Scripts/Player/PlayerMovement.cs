using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;

    private Vector2 _movementInput;
    Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Run();
    }

    private void Run()
    {
        Vector3 movement = new Vector3(_movementInput.x * walkSpeed, _rigidbody.velocity.y, _movementInput.y * walkSpeed);
        _rigidbody.velocity = transform.TransformDirection(movement);
    }

    private void OnMove(InputValue value)
    {
        _movementInput = value.Get<Vector2>();
    }
}