using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawStrike : MonoBehaviour
{
    [SerializeField] private Animator _clawsAnimator;

    private Animator _playerAnimator;

    private void Start()
    {
        _playerAnimator = GetComponent<Animator>();     
    }

    public void Strike() 
    {
        _clawsAnimator.SetTrigger("strike");
        _playerAnimator.SetTrigger("is_attack");
    }

}
