using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextOpacityController : MonoBehaviour
{
    public TextMeshProUGUI[] texts; // Array to hold the TextMeshProUGUI objects

    private int currentIndex = 0; // Index of the currently fading text

    private float fadeSpeed = 0.5f; // Speed at which the opacity changes
    private float targetOpacity = 1f; // The target opacity value (fully opaque)

    private void Start()
    {
        StartCoroutine(FadeTexts());
    }

    private IEnumerator FadeTexts()
    {
        // Loop through the texts array
        for (int i = 0; i < texts.Length; i++)
        {
            // Set the current text to the target opacity (fully opaque)
            texts[currentIndex].color = new Color(texts[currentIndex].color.r, texts[currentIndex].color.g, texts[currentIndex].color.b, targetOpacity);

            // Increase the index to move to the next text
            currentIndex++;

            // Wait for a short duration before fading the next text
            yield return new WaitForSeconds(0.8f);

            // Reset the index if it exceeds the array length
            if (currentIndex >= texts.Length)
            {
                currentIndex = 0;
            }
        }
    }
}
