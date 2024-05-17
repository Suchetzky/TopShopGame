using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// public class ComboAnimation : MonoBehaviour
// {
//     [Range(0.01f, 1f)]
//     [SerializeField] private float gapTime=0.05f;
//     [SerializeField] private Sprite[] red3Sprites;
//     [SerializeField] private Sprite[] blue5Sprites;
//
//     private Image _image;
//     private int _combo=0;
//     public static ComboAnimation Shared { get; private set; }
//     private void Awake()
//     {
//         Shared = this;
//     }
//
//     private void Start()
//     {
//         // StartCoroutine(SwitchSprite(red3Sprites));
//         _image = GetComponent<Image>();
//     }
//
//     private void Update()
//     {
//         if(_combo == 3)
//             StartCoroutine(SwitchSprite(red3Sprites));
//         else if(_combo == 5)
//             StartCoroutine(SwitchSprite(blue5Sprites));
//     }
//
//
//     public void PlayComboAnimation(int combo)
//     {
//         _combo = combo;
//     }
//
//     private IEnumerator SwitchSprite(Sprite[] sprites)
//     {
//         for (int i = 0; i < sprites.Length; i++)
//         {
//             print("switching sprite");
//             _image.sprite = sprites[i];
//             yield return new WaitForSeconds(gapTime);
//         }
//     }
//     
// }



public class ComboAnimation : MonoBehaviour
{
    public Sprite[] images;     // Array of sprites to loop through

    private Image imageComponent;
    private int currentIndex = 0;
    

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
    }

    private void OnEnable()
    {
        LoopImagesOnce();
    }

    private void LoopImagesOnce()
    {
        
        if (images.Length == 0)
        {
            return;
        }
        
        // Start the loop coroutine
        StartCoroutine(LoopImages());
    }

    private IEnumerator LoopImages()
    {
        // Loop through all the sprites in the array
        for (int i = 0; i < images.Length; i++)
        {
            // Set the current sprite
            imageComponent.sprite = images[i];

            // Wait for a short interval
            yield return new WaitForSeconds(0.08f);
        }

        // Reset the image to the first sprite after the loop finishes
        imageComponent.sprite = images[0];
    }
}
