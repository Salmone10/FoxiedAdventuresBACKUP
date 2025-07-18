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
    public bool _isCanMove = true;
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
    [SerializeField] public bool _isRollLocked = false;

    [Header("Interactions")]
    public float _interactionRadius;
    [SerializeField] public bool _isInteract = false;

    [Header("Snake player")]
    public float _raycastLenth;
    public LayerMask _raycastLayerMaskColission;
    public bool _isCanMoveOnWall;
    public bool _isClimbingOnWall;

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
    public Vector2 Direction { set => _direction = value; get => _direction; }

    [SerializeField] private CheckGround _checkGround;
    public CheckGround CheckGround { get => _checkGround; }
    public Collider2D[] _interactionResult = new Collider2D[1];
    [SerializeField] public ReloadScale _reloadScale;

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
        
        if (!_isRollLocked) { _canRoll = _checkGround._ground && _direction.x == 0 && !_isRoll; }

        if (MathF.Abs(y_location) < infelicity) { y_location = 0f; }

        _animator.SetFloat("y_location", y_location);

        if (_isRoll) return;

        if (_direction.x != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(_direction.x), 1); //поворот игрока
        }

        if (!gameObject.CompareTag("SquirrelPlayer")) { _animator.SetBool("is_on_ladder", _checkLadder._ladder); }
        
        _animator.SetBool("is_ground", _checkGround._ground);
        _animator.SetBool("is_running", _direction.x != 0);

        if (_isCanMove)
        {
            Move();
        }
        else
        {
            _rigidBody.velocity = new Vector2(0f, _rigidBody.velocity.y);
            _animator.SetBool("is_running", false);
        }

        CheckWall();
        WallClimb();
    }

    void FixedUpdate()
    {
        _animator.SetBool("is_rolling", _isRoll);
        Ladder();
    }

    public void CheckWall() 
    {
        if (gameObject.CompareTag("SnakePlayer")) 
        {
            var rayDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            var wallhit = Physics2D.Raycast(transform.position, rayDirection, _raycastLenth, _raycastLayerMaskColission);
            Debug.DrawRay(transform.position, rayDirection*_raycastLenth, Color.yellow);
            _isCanMoveOnWall = wallhit.collider != null ? true : false;
        } 
    }

    public void WallClimb() 
    {
        if (_isCanMoveOnWall && _direction.x != 0)
        {
            _isClimbingOnWall = true;
            _rigidBody.gravityScale = 0;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _speed);
        }
        else 
        {
            _isClimbingOnWall = false;
            _rigidBody.gravityScale = 5;
        }
    }

    private void Move()
    {
        _rigidBody.velocity = new Vector2(_speed * _direction.x, _rigidBody.velocity.y);
    }

    public void CommonJump()
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
        if (!gameObject.CompareTag("SquirrelPlayer") || !gameObject.CompareTag("SnakePlayer")) 
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
    public void TakeDamage()
    {
        _animator.SetTrigger("is_hurt");
        _rigidBody.AddForce(Vector2.up * _knockbackForce, ForceMode2D.Impulse);
    }

    public void TakeHeal()
    {
        StartCoroutine(ChangeColor());
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

    #region Other
    IEnumerator RollingCrt()
    {
        _isRoll = true;

        print(_isRoll);
        var rollDirection = Mathf.Sign(transform.lossyScale.x);
        _rigidBody.velocity = new Vector2(_rollSpeed * rollDirection, _rigidBody.velocity.y);

        yield return new WaitForSeconds(_rollTime);

        _isRoll = false;

        _reloadScale.Reload();
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

    public void DisableMove()
    {
        print("DisableMove");
        
        _isCanMove = false;
    }

    public void EnableMove()
    {
        print("EnableMove");
        _isCanMove = true;
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
    #endregion
}