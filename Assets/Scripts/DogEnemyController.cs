using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnemyController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _movementSpeed;

    [SerializeField] private float _minWaitTime;
    [SerializeField] private float _maxWaitTime;

    [SerializeField] private bool _isWaiting = false;

    [SerializeField] private Transform[] _wayPoint;
    private int _currentIndexOfPoint = 0;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!_isWaiting)
        {
            Patrol();
        }

    }

    public void SpriteFlip(Vector2 direction)
    {
        if (direction.x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (direction.x < 0)
        {
            _spriteRenderer.flipX = false;
        }
    }

    public void Patrol()
    {
        var targetPosition = _wayPoint[_currentIndexOfPoint].position;
        var newPosition = Vector2.MoveTowards(_rigidBody.position, targetPosition, _movementSpeed * Time.fixedDeltaTime);
        var direction = ((Vector2)targetPosition - _rigidBody.position).normalized;

        SpriteFlip(direction);
        _animator.SetBool("is_running", direction.x != 0);

        _rigidBody.MovePosition(newPosition);

        if (Vector2.Distance(_rigidBody.position, targetPosition) < 1)
        {
            _currentIndexOfPoint = (_currentIndexOfPoint + 1) % _wayPoint.Length;
            StartCoroutine(WaitCrt());
        }
    }

    IEnumerator WaitCrt()
    {
        _isWaiting = true;
        var waitingTime = Random.Range(_minWaitTime, _maxWaitTime);
        _animator.SetBool("is_running", false);
        yield return new WaitForSeconds(waitingTime);
        _isWaiting = false;
    }
}
