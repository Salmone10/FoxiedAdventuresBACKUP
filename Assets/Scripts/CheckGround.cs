using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    [SerializeField] public bool _ground;

    private BoxCollider2D _boxCollider2D;
    public LayerMask _groundMask;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _ground = _boxCollider2D.IsTouchingLayers(_groundMask);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _ground = _boxCollider2D.IsTouchingLayers(_groundMask);
    }
}
