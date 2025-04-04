using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchComponent : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animatorParametre;
    [SerializeField] private bool _switch;

    public void Switch() 
    {
        _switch = !_switch;
        _animator.SetBool(_animatorParametre, _switch);
    }
}
