using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // настрйока персонажа
    public float _speed;
    public float _jumpPower;
    public float _knockbackForce;
    public float _climbingSpeed;
    public float _rollSpeed;

    public float _colorFadeTime;
    public float _blinkingSpeed;
    public float _blinkingTime;

    public float _rollTime;

    public float _interactionRadius;

    private Color _origColor;

    [SerializeField] public bool _isRoll = false;
    [SerializeField] public bool _isClimbing = false;
    [SerializeField] public bool _isInteract = false;

    // компоненты
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private CheckGround _checkGround;
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
        string testJson = "{\"score\":5}";

        GameData testData = JsonUtility.FromJson<GameData>(testJson);

        if (testData != null)

            Debug.Log("Тестовая загрузка успешна: " + testData.score);

        else

            Debug.LogError("Ошибка: JsonUtility вернул null");
    }

    private void Update()
    {
        _animator.SetFloat("y_location", _rigidBody.velocity.y);

        if (_isRoll) return;

        if (_direction.x != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(_direction.x), 1); //поворот игрока
        }

        _animator.SetBool("is_on_ladder", _checkLadder._ladder);
        
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
        if (!_isRoll && _checkGround._ground)
        {
            StartCoroutine(RollingCrt());
        }
    }

    public void Ladder()
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
                print("interact");
                interactObject.InteractEvent();
            }
        }

    }

    IEnumerator RollingCrt()
    {
        _isRoll = true;
        print(_isRoll);
        var rollDirection = Mathf.Sign(transform.lossyScale.x);
        _rigidBody.velocity = new Vector2(_rollSpeed * rollDirection, _rigidBody.velocity.y);

        yield return new WaitForSeconds(_rollTime);
        _isRoll = false;
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