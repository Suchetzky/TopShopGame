using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMakeCartsScript : MonoBehaviour
{
    public List<GameObject> carts; // List of gameobjects representing the carts

    private bool cartsEnabled = false; // Flag to track if the carts are enabled

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnableCartsAfterDelay(11f)); // Start the coroutine to enable the carts after 3 seconds
    }

    // Update is called once per frame
    void Update()
    {
        // Your other code here...
    }

    IEnumerator EnableCartsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Enable all the carts
        foreach (GameObject cart in carts)
        {
            cart.SetActive(true);
        }

        cartsEnabled = true; // Set the flag to indicate that the carts are enabled
    }
}