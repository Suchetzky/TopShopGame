using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    public Sprite[] firstSetImages;    // Array of sprites for the first set of images
    public Sprite[] secondSetImages;   // Array of sprites for the second set of images
    public Sprite[] thirdSetImages;    // Array of sprites for the third set of images
    public Sprite[] fourthSetImages;   // Array of sprites for the fourth set of images
    public float firstSetSwitchInterval = 1f;   // Time interval between switches for the first set
    public float secondSetSwitchInterval = 0.5f;   // Time interval between switches for the second set
    public float thirdSetSwitchInterval = 0.5f;   // Time interval between switches for the third set
    public float fourthSetSwitchInterval = 0.5f;  // Time interval between switches for the fourth set

    private Image imageComponent;
    private int firstSetIndex = 0;
    private int secondSetIndex = 0;
    private int thirdSetIndex = 0;
    private int fourthSetIndex = 0;
    private Coroutine secondSetCoroutine;
    private Coroutine thirdSetCoroutine;
    private Coroutine fourthSetCoroutine;

    public static ImageSwitcher instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        imageComponent = GetComponent<Image>();

        // Start the image switching coroutine for the first set
        StartCoroutine(SwitchFirstSetImages());
    }

    private IEnumerator SwitchFirstSetImages()
    {
        // Loop through all the sprites in the first set except the last one
        for (int i = 0; i < firstSetImages.Length - 1; i++)
        {
            // Set the current sprite
            imageComponent.sprite = firstSetImages[i];

            // Wait for the specified time interval
            yield return new WaitForSeconds(firstSetSwitchInterval);
        }

        // Set the last sprite of the first set
        imageComponent.sprite = firstSetImages[firstSetImages.Length - 1];

        // Start the image switching coroutine for the second set
        secondSetCoroutine = StartCoroutine(SwitchSecondSetImages());
    }

    private IEnumerator SwitchSecondSetImages()
    {
        while (true)
        {
            // Set the current sprite of the second set
            imageComponent.sprite = secondSetImages[secondSetIndex];

            // Move to the next sprite index
            secondSetIndex = (secondSetIndex + 1) % secondSetImages.Length;

            // Wait for the specified time interval
            yield return new WaitForSeconds(secondSetSwitchInterval);
        }
    }

    private IEnumerator SwitchThirdSetImages()
    {
        // Stop the second set coroutine if it's running
        if (secondSetCoroutine != null)
            StopCoroutine(secondSetCoroutine);

        // Loop through all the sprites in the third set
        for (int i = 0; i < thirdSetImages.Length; i++)
        {
            // Set the current sprite of the third set
            imageComponent.sprite = thirdSetImages[i];

            // Wait for the specified time interval
            yield return new WaitForSeconds(thirdSetSwitchInterval);
        }

        // Set the last sprite of the third set
        imageComponent.sprite = thirdSetImages[thirdSetImages.Length - 1];
    }

    private IEnumerator SwitchFourthSetImages()
    {
        // Stop the third set coroutine if it's running
        if (thirdSetCoroutine != null)
            StopCoroutine(thirdSetCoroutine);

        // Loop through all the sprites in the fourth set
        for (int i = 0; i < fourthSetImages.Length; i++)
        {
            // Set the current sprite of the fourth set
            imageComponent.sprite = fourthSetImages[i];

            // Wait for the specified time interval
            yield return new WaitForSeconds(fourthSetSwitchInterval);
        }

        // Set the last sprite of the fourth set
        imageComponent.sprite = fourthSetImages[fourthSetImages.Length - 1];
    }

    public void PlayThirdSet()
    {
        // Start the image switching coroutine for the third set
        thirdSetCoroutine = StartCoroutine(SwitchThirdSetImages());
    }

    public void PlayFourthSet()
    {
        // Start the image switching coroutine for the fourth set
        fourthSetCoroutine = StartCoroutine(SwitchFourthSetImages());
    }
}
