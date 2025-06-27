using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class VenomSpit : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidBody;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (_rigidBody.velocity.sqrMagnitude > 0.01) 
        {
            var angle = Mathf.Atan2(_rigidBody.velocity.y, Mathf.Abs(_rigidBody.velocity.x)) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? angle : -angle);
            transform.localScale = new Vector3(Mathf.Sign(_rigidBody.velocity.x), 1, 1);
        }   
    } 
}
