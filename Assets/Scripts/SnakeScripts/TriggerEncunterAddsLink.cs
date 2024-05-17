using System.Collections.Generic;
using UnityEngine;

public class TriggerEncunterAddsLink : MonoBehaviour
{
    private Dictionary<string, int> _colorDictionary;


    private void Start()
    {
        _colorDictionary = GameManager.Shared.colorDictionary;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("BasicCart") &&  !_colorDictionary.ContainsKey(other.transform.tag))
            return;
        if (other.GetComponent<TriggerEncunterAddsLink>()) return;
        
        snakeMovmentScript.instance.AddBodyPart(transform.tag);
        Destroy(gameObject);
    }
}
