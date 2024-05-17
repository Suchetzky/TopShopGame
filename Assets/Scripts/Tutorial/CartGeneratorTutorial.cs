using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartGeneratorTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cartPrefab1; // Prefab of the first cart
    public GameObject cartPrefab2; // Prefab of the second cart

    public Vector3 spawnRangeMin; // Minimum x, y, z values for spawning
    public Vector3 spawnRangeMax; // Maximum x, y, z values for spawning

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateCartEveryTenSeconds());
    }

    IEnumerator GenerateCartEveryTenSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);

            // Generate a random position within the given range
            Vector3 spawnPosition = new Vector3(
                Random.Range(spawnRangeMin.x, spawnRangeMax.x),
                1.3f,
                Random.Range(spawnRangeMin.z, spawnRangeMax.z)
            );

            // Generate a random number to choose one of the two cart prefabs
            int randomCart = Random.Range(0, 2);

            // Instantiate the chosen cart at the generated position
            if (randomCart == 0)
            {
                Instantiate(cartPrefab1, spawnPosition, Quaternion.identity);
            }
            else
            {
                Instantiate(cartPrefab2, spawnPosition, Quaternion.identity);
            }
        }
    }
}
