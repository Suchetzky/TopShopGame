using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CartBlinkAndDestroy : MonoBehaviour
{
    [SerializeField] float blinkDuration = 0.3f;  // Duration of each blink in seconds
    [SerializeField] int blinkCount = 10;          // Number of times the object will blink
    [SerializeField] float destroyDelay = 0.2f;   // Delay before destroying the object after blinking

    private Transform cart;
    private Transform cartWheels;
    private bool isBlinking = false;
    private float timeToLive;

    private void Start()
    {
        cart = transform.GetChild(0);
        cartWheels = transform.GetChild(1);
        if (cart == null || cartWheels == null)
        {
            Debug.LogError("BlinkAndDestroy script requires a cart child.");
            enabled = false;
        }
        timeToLive = Random.Range(20f, 30f);
    }

    private void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0 && !isBlinking)
        {
            StartBlinking();
        }
    }

    private void StartBlinking()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            StartCoroutine(BlinkCoroutine());
        }
    }

    private System.Collections.IEnumerator BlinkCoroutine()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            cart.gameObject.SetActive(false);
            cartWheels.gameObject.SetActive(false);
            yield return new WaitForSeconds(blinkDuration);
            cart.gameObject.SetActive(true);
            cartWheels.gameObject.SetActive(true);
            yield return new WaitForSeconds(blinkDuration);
            
            // Decrease blink duration over time
            blinkDuration *= 0.95f;
        }
        RandomizeCartsMaker.instance.ReduceCart();
        Destroy(gameObject);
    }
}
