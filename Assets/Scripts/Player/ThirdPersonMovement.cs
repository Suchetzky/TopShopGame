using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float turnSpeed = 0.1f;
    [SerializeField] private float cartTurnSpeed = 1f;
    [SerializeField] private float spinAngle = 1f;
    [SerializeField] private GameObject toMove;
    [SerializeField] private Image X;
    [SerializeField] private ParticleSystem dust;
    private PlayerInput _playerInput;
    private Vector2 _cartMovementInputRight;
    private Vector2 _cartMovementInputLeft;
    private Vector2 _movementInput;
    private Vector2 _rightInput;
    private Vector2 _leftInput;
    private Vector3 _rightCameraVector;
    private Rigidbody _rigidbody;
    private int _currentActionMapIndex = 0;
    private bool _switchActionMap = false;
    private bool _isCart = false;
    private float _factor = 0f;
    private string[] _actionsMap;
    
    public static ThirdPersonMovement instance;
    [SerializeField] private float boostAmount;
    public float timeForBoost = 0.2f;
    [SerializeField] private float initialTimeBetweenBoosts = 2f;
    private float _timeBetweenBoosts = 2f;
    private bool _boosted;
    public bool gotHit;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _actionsMap = new[] { "Player", "CartMovement" };
        _rigidbody = toMove.GetComponent<Rigidbody>();
        Cursor.visible = false;
    }
    
    private void SwitchActionMap()
    {
        _switchActionMap = false;
        _currentActionMapIndex++;
        _currentActionMapIndex %= _actionsMap.Length;
        Debug.Log(_actionsMap[_currentActionMapIndex]);
        if (_actionsMap[_currentActionMapIndex] == "CartMovement")
            _isCart = true;
        else
            _isCart = false;
        _playerInput.SwitchCurrentActionMap(_actionsMap[_currentActionMapIndex]);
    }

    private void Update()
    {
        Move();
        if (_boosted) ReduceTimeBtwBoosts();
    }

    private void Move()
    {
        if (gotHit) return;
        Transform camera = Camera.main.transform;
        Vector3 forward = camera.forward;
        Vector3 right = camera.right;
        forward.y = 0f;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;
        if (_isCart)
        {
            _movementInput = _rightInput + _leftInput;
            _movementInput = _movementInput.normalized;
        }

        Vector3 movement = forward * _movementInput.y + right * (spinAngle * _movementInput.x);
        if(!movement.Equals( Vector3.zero))
        {
            toMove.transform.rotation = Quaternion.Slerp(toMove.transform.rotation, Quaternion.LookRotation(movement* cartTurnSpeed ), turnSpeed);
        }
        toMove.transform.Translate(movement * (Time.deltaTime * walkSpeed), Space.World);
    }

    private void OnMove(InputValue value)
    {
        _movementInput = value.Get<Vector2>();
    }
    
    private void OnBoost(InputValue value)
    {
        if (!(value.Get<float>() > 0) || _boosted || _movementInput == Vector2.zero) return;
        walkSpeed += boostAmount;
        _boosted = true;
        dust.Play();
        StartCoroutine(FillProgressBarCoroutine());
        StartCoroutine(ReturnSpeedToNormal());
    }

    private IEnumerator ReturnSpeedToNormal()
    {
        yield return new WaitForSeconds(timeForBoost);
        if (walkSpeed - boostAmount > 0) walkSpeed -= boostAmount;
    }

    private void OnCartMoveRight(InputValue value)
    {
        _cartMovementInputRight = value.Get<Vector2>();
        float fixedX,fixedY;
        if (!_cartMovementInputRight.Equals(Vector2.zero))
        {
            fixedY = Mathf.Asin(_cartMovementInputRight.y) + Mathf.PI * 0.25f;
            fixedX = Mathf.Acos(_cartMovementInputRight.x) + Mathf.PI * 0.25f;
            _rightInput = new Vector2(Mathf.Cos(fixedX), Mathf.Sin(fixedY));
        }
        else
            _rightInput = Vector2.zero;
    }
    private void OnCartMoveLeft(InputValue value)
    {
        _cartMovementInputLeft = value.Get<Vector2>();
        float fixedX = 0,fixedY = 0;
        if(!_cartMovementInputLeft.Equals(Vector2.zero))
        {
            fixedY = Mathf.Asin(_cartMovementInputLeft.y) - Mathf.PI * 0.25f;
            fixedX = Mathf.Acos(_cartMovementInputLeft.x) - Mathf.PI * 0.25f;
            _leftInput = new Vector2(Mathf.Cos(fixedX), Mathf.Sin(fixedY));
        }
        else
            _leftInput = Vector2.zero;
    }

    public void ChangeGameObjectToMove(GameObject newObject)
    {
        toMove = newObject;
        _rigidbody = toMove.GetComponent<Rigidbody>();
    }

    public bool IsMove()
    {
        return !_movementInput.Equals(Vector2.zero);
    }
    
    private void OnSwitch(InputValue value)
    {
        _switchActionMap = true;
    }

    private void ReduceTimeBtwBoosts()
    {
        if (_timeBetweenBoosts > 0) _timeBetweenBoosts -= Time.deltaTime;
        else
        {
            _boosted = false;
            _timeBetweenBoosts = initialTimeBetweenBoosts;
        }
    }
    
    private IEnumerator FillProgressBarCoroutine()
    {
        float elapsedTime = 0f;
        X.gameObject.SetActive(true);
        while (elapsedTime < initialTimeBetweenBoosts)
        {
            float progress = 1-(elapsedTime / initialTimeBetweenBoosts);
            X.fillAmount = progress;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        X.gameObject.SetActive(false);
    }

    private void OnPush()
    {
        snakeMovmentScript.instance.ExplodeAll();
    }

    public void SetSpeed(float speed) {walkSpeed = speed;}
}
