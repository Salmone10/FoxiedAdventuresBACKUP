using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixCollisionChanger : MonoBehaviour
{
    [SerializeField] private Collider2D _player;
    private Collider2D _object;

    private void Start()
    {
        _object = GetComponent<Collider2D>();
    }

    public void Changer()
    {
        Physics2D.IgnoreCollision(_object, _player);
    }

}
