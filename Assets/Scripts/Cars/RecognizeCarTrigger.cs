using System;
using UnityEngine;

public class RecognizeCarTrigger : MonoBehaviour
{
    private OpenCloseGate _gate;

    private void Awake()
    {
        _gate = GetComponentInChildren<OpenCloseGate>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var car  = other.GetComponent<Car>();
        if (!car) return;
        if(car.IsOut()) car.GetSmall();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Car"))
        {
            _gate.OpenGate();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.transform.CompareTag("Car")) return;
        _gate.CloseGate();
        // var car  = other.GetComponent<Car>();
        // if (!car) return;
        // if(car.IsOut()) car.GetSmall();
    }
}
