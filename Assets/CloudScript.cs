using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudScript : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private float _speed;
    private int _rotation;
    private void Start()
    {
        _speed = Random.Range(2f, 5f);
        _rotation = Random.Range(0, 2);
        if (_rotation == 0) _rotation = -1;
    }

    void Update()
    {
        // Spin the object around the target at 20 degrees/second.
        transform.RotateAround(target.transform.position, Vector3.up, _speed * _rotation * Time.deltaTime);
    }
}
