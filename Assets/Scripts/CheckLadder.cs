using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLadder : MonoBehaviour
{
    [SerializeField] public bool _ladder;

    private Collider2D _collider2D;
    public LayerMask _ladderMask;

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _ladder = _collider2D.IsTouchingLayers(_ladderMask);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _ladder = _collider2D.IsTouchingLayers(_ladderMask);
    }

}
