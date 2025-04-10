using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawStrike : MonoBehaviour
{
    private Animator _animator;

    public void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Strike() 
    {
        _animator.SetTrigger("strike");
    }

}
