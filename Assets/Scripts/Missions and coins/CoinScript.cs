using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    
    Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
        Destroy(gameObject, 1);
    }
    private void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }

}
