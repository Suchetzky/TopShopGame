using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combos : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Property = Animator.StringToHash("3");
    private static readonly int Property1 = Animator.StringToHash("5");

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ShowThreeCombo()
    {
        _animator.SetTrigger(Property);
    }
    
    public void ShowFiveCombo()
    {
        _animator.SetTrigger(Property1);
    }
}
