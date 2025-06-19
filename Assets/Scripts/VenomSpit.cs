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

    private void Update()
    {
        var y_location = _rigidBody.velocity.y;
        _animator.SetFloat("y_location", y_location);

        if (_rigidBody.velocity.sqrMagnitude > 0.01f) 
        {
            var direction = _rigidBody.velocity;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (_rigidBody.velocity.x < 0) { transform.localScale = new Vector3(1, -1, 1); }

            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
