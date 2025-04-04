using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INT_CrateFall : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    public void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Fall()
    {
        _rigidbody.simulated = true;
    }
}
