using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalEffects : MonoBehaviour
{
    [SerializeField] float _sideImpulse;
    [SerializeField] float _upImpulse;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _circleCollider2D;

    private DogEnemyController _enemyController;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _enemyController = GetComponent<DogEnemyController>();
    }

    public void MakeImpulse()
    {
        var impulseDirection = Random.value < 0.5f ? -1f : 1f;
        var impulse = new Vector2(impulseDirection * _sideImpulse, _upImpulse);

        _rigidbody2D.AddForce(impulse, ForceMode2D.Impulse);
        _spriteRenderer.flipY = true;
        _circleCollider2D.isTrigger = true;
        _enemyController.enabled = false;
        
    }

}
