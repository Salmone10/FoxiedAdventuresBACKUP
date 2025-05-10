using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Common")]
    public float _speed;
    public float _jumpPower;
    public float _knockbackForce;
    public float _climbingSpeed; // dont include Squirrel
    [SerializeField] public bool _isClimbing = false;

    [Header("Damage (hurt) eff settings")]
    public float _colorFadeTime;
    public float _blinkingSpeed;
    public float _blinkingTime;

    [Header("Squirrel player")]
    public float _rollTime;
    public float _rollSpeed;
    [SerializeField] public bool _isRoll = false;
    [SerializeField] public bool _canRoll = true;

    [Header("Interactions")]
    public float _interactionRadius;
    [SerializeField] public bool _isInteract = false;

    private Color _origColor;

    // components
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    [SerializeField] private ContactFilter2D _interactLayer;
    
    private CheckLadder _checkLadder;
    private FireballController _fireballController;
    private AudioSource _audioSource;
    private PlayerStatistic _playerStatistic;
    private HealthManager _healthManager;
    private EnemySaver _enemySaver;

    public float _tempSpeed;

    private Vector2 _direction;
    public Vector2 Direction { set => _direction = value; }

    [SerializeField] private CheckGround _checkGround;
    public CheckGround CheckGround { get => _checkGround; }
    public Collider2D[] _interactionResult = new Collider2D[1];

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _checkLadder = GetComponent<CheckLadder>();
        _audioSource = GetComponent<AudioSource>();
        _playerStatistic = GetComponent<PlayerStatistic>();
        _healthManager = GetComponent<HealthManager>();
        _enemySaver = GetComponent<EnemySaver>();
        _origColor = _spriteRenderer.color;
        _tempSpeed = _speed;
    }

    private void Update()
    {   
        var infelicity = 0.1f;
        var y_location = _rigidBody.velocity.y;
        _canRoll = true;

        if (MathF.Abs(y_location) < infelicity) 
        {
            y_location = 0f;
        }

        _animator.SetFloat("y_location", y_location);

        if (_isRoll) return;

        if (_direction.x != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(_direction.x), 1); //поворот игрока
            _canRoll = false;
        }

        if (!gameObject.CompareTag("SquirrelPlayer")) { _animator.SetBool("is_on_ladder", _checkLadder._ladder); }
        
        _animator.SetBool("is_ground", _checkGround._ground);
        _animator.SetBool("is_running", _direction.x != 0);

        Move();
        
    }

    private void Move()
    {
        _rigidBody.velocity = new Vector2(_speed * _direction.x, _rigidBody.velocity.y);
    }

    public void Jump()
    {
        if (_checkGround._ground && !_isRoll)
        {
            _rigidBody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    public void Roll()
    {
        if (!_isRoll && _checkGround._ground && _canRoll)
        {
            StartCoroutine(RollingCrt());
        }
    }

    public void Ladder()
    {
        if (!gameObject.CompareTag("SquirrelPlayer")) 
        {
            if (_checkLadder._ladder)
            {
                _rigidBody.gravityScale = 0;
                _rigidBody.velocity = new Vector2(_speed * _direction.x, _climbingSpeed * _direction.y);
            }
            else
            {
                _rigidBody.gravityScale = 5;
            }
        }
    }

    void FixedUpdate()
    {
        _animator.SetBool("is_rolling", _isRoll);
        Ladder();
    }

    public void TakeDamage()
    {
        _animator.SetTrigger("is_hurt");
        _rigidBody.AddForce(Vector2.up * _knockbackForce, ForceMode2D.Impulse);
    }

    public void TakeHeal()
    {
        StartCoroutine(ChangeColor());
    }

    public void Taunt() 
    {
        if (_checkGround)
        {
            _animator.SetTrigger("is_taunting");
            _audioSource.Play();
        }
    }

    public void Interact()
    {
        int hit = Physics2D.OverlapCircle(transform.position, _interactionRadius, _interactLayer, _interactionResult);

        bool isHit = hit > 0;

        print($"{hit}, {isHit}");

        if (isHit)
        {
            var interactObject = _interactionResult[0].GetComponent<InteractComponent>();

            if (interactObject != null)
            {
                interactObject.InteractEvent();
            }
        }

    }

    IEnumerator RollingCrt()
    {
        _isRoll = true;
        _canRoll = false;

        print(_isRoll);
        var rollDirection = Mathf.Sign(transform.lossyScale.x);
        _rigidBody.velocity = new Vector2(_rollSpeed * rollDirection, _rigidBody.velocity.y);

        yield return new WaitForSeconds(_rollTime);

        _isRoll = false;
        _canRoll = true;
    }

    IEnumerator ChangeColor()
    {
        _spriteRenderer.color = Color.green;
        float timePassed = 0;

        while (timePassed < _colorFadeTime)
        {
            _spriteRenderer.color = Color.Lerp(Color.green, _origColor, timePassed / 2);
            timePassed += Time.deltaTime;

            yield return null;
        }
        _spriteRenderer.color = _origColor;
    }

    IEnumerator BlinckingCrt()
    {
        float timePassed = 0;


        while (timePassed < _blinkingTime)
        {
            timePassed += Time.deltaTime + _blinkingSpeed * 2;
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(_blinkingSpeed);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(_blinkingSpeed);

        }
        _spriteRenderer.enabled = true;
    }

    public void Blinkering()
    {
        StartCoroutine(BlinckingCrt());
    }

    public void SaveGame() 
    {
        print(_enemySaver._enemies.Length);
        var data = new GameData(_playerStatistic._score, transform.position, _healthManager._currentHealth, _enemySaver._enemies);
        SaveSystem.Save(data);
    }

    public void LoadGame() 
    {
        var data = SaveSystem.Load();
        if (data != null) 
        {
            transform.position = data.playerPositon;
            _playerStatistic._score = data.score;
            _healthManager._currentHealth = data.health;
        }
    }
}