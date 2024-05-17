using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private ThirdPersonMovement movement;

    private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.speed = movement.IsMove() ? 1 : 0;
    }   
}
