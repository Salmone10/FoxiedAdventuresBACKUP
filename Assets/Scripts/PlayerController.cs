using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    [SerializeField] public Transform _raycatPoint;
    public float _pauseBfrClimbing;
    private float _pauseBfrClimbingTime;

    [Header("Dash")]
    public float _dashTime;
    public float _dashSpeed;
    public float _beforeDashAnimationTiming;
    [SerializeField] private LayerMask _dashLayers;
    private Dictionary<int, bool> _layerIgnores = new();
    


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

    [SerializeField] private enum WallSide { None, Left, Right };
    [SerializeField] private WallSide _wallSide = WallSide.None;

    [Header("Snake jump")]

    [SerializeField] private int _maxJumps;
    [SerializeField] private float _minJumpImpuls;
    [SerializeField] private float _maxJumpImpuls;
    [SerializeField] private float _minChargeTime;
    [SerializeField] private float _maxChargeTime;
    [SerializeField] private float _chargeUIShowDelay;
    [SerializeField] private Slider _chargeSlider;

    private bool _isChargingJump;
    private float _chargeTime;
    private float _uiShowTime;
    private int _counterJump;


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

        _counterJump = _maxJumps;
    }

    private void Update()
    {   
        var infelicity = 0.1f;
        var y_location = _rigidBody.linearVelocity.y;
        
        if (!_isRollLocked) { _canRoll = _checkGround._ground && _direction.x == 0 && !_isRoll; }
        if (_checkGround._ground) { _spriteRenderer.flipY = false; }
        if (MathF.Abs(y_location) < infelicity) { y_location = 0f; }

        _animator.SetFloat("y_location", y_location);

        if (_isRoll) return;

        if (_direction.x != 0)
        {
            if (!_isClimbingOnWall)
            {
                transform.localScale = new Vector2(Mathf.Sign(_direction.x), 1); //������� ������
            }
            else 
            {
                if (_direction.x > 0 && _wallSide == WallSide.Left) { _spriteRenderer.flipY = true; }
                else if (_direction.x < 0 && _wallSide == WallSide.Right) { _spriteRenderer.flipY = true; }
            }
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
            _rigidBody.linearVelocity = new Vector2(0f, _rigidBody.linearVelocity.y);
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
        if (!gameObject.CompareTag("SnakePlayer")) return;

        var rightOrigin = _raycatPoint.position + new Vector3(0.25f, 0f);
        var leftOrigin = _raycatPoint.position - new Vector3(0.25f, 0f);

        var hitRight = Physics2D.Raycast(rightOrigin, Vector2.right, _raycastLenth, _raycastLayerMaskColission);
        var hitLeft = Physics2D.Raycast(leftOrigin, Vector2.left, _raycastLenth, _raycastLayerMaskColission);

        Debug.DrawRay(rightOrigin, Vector2.right * _raycastLenth, Color.yellow);
        Debug.DrawRay(leftOrigin, Vector2.left * _raycastLenth, Color.yellow);

        if (hitRight.collider != null) { _wallSide = WallSide.Right; _isCanMoveOnWall = true; }
        else if (hitLeft.collider != null) { _wallSide = WallSide.Left; _isCanMoveOnWall = true; }
        else { _wallSide = WallSide.None; _isCanMoveOnWall = false; }

        var pressingTowardsWall = (_wallSide == WallSide.Right && _direction.x > 0) || (_wallSide == WallSide.Left && _direction.x < 0);
            
        if (_checkGround._ground)
        {
            if (_isClimbingOnWall)
            {
                if (Time.time >= _pauseBfrClimbingTime && !pressingTowardsWall)
                    ExitWall();
            }
            else
            {
                if (_isCanMoveOnWall && pressingTowardsWall)
                    EnterWall();
            }
            return;
        }

        if (_isCanMoveOnWall)
        {
            if (!_isClimbingOnWall && pressingTowardsWall)
                EnterWall();
        }
        else
        {
            if (_isClimbingOnWall)
                ExitWall();
        }
    }

    private void EnterWall()
    {
        _isClimbingOnWall = true;
        _animator.SetBool("is_climbing_on_wall", _isClimbingOnWall);
        _rigidBody.gravityScale = 0;
        _pauseBfrClimbingTime = Time.time + _pauseBfrClimbing;
    }

    private void ExitWall()
    {
        _isClimbingOnWall = false;
        _isCanMoveOnWall = false;
        _wallSide = WallSide.None;
        _animator.SetBool("is_climbing_on_wall", _isClimbingOnWall);
        _rigidBody.gravityScale = 5;
    }


    public void WallClimb() 
    {
        if (!_isClimbingOnWall) return;

        var isRightWall = _wallSide == WallSide.Right;
        var isLeftWall = _wallSide == WallSide.Left;

        var upWall = (isRightWall && _direction.x > 0) || (isLeftWall && _direction.x < 0);
        var downWall = (isRightWall && _direction.x < 0) || (isLeftWall && _direction.x > 0);

        if (upWall)
        {
            _rigidBody.linearVelocity = new Vector2(0f, _speed * Mathf.Abs(_direction.x));
            _spriteRenderer.flipY = false;
        }
        else if (downWall)
        {
            _rigidBody.linearVelocity = new Vector2(0f, -_speed * Mathf.Abs(_direction.x));
        }
        else
        {
            _rigidBody.linearVelocity = Vector2.zero;
        }
    }

    private void Move()
    {
        _rigidBody.linearVelocity = new Vector2(_speed * _direction.x, _rigidBody.linearVelocity.y);
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

    public void Dash()
    {
        if (!_isRoll && _checkGround._ground && _canRoll)
        {
            StartCoroutine(DashCrt());
        }
    }

    public void Ladder()
    {
        if (!gameObject.CompareTag("SquirrelPlayer") && !gameObject.CompareTag("SnakePlayer")) 
        {
            if (_checkLadder._ladder)
            {
                _rigidBody.gravityScale = 0;
                _rigidBody.linearVelocity = new Vector2(_speed * _direction.x, _climbingSpeed * _direction.y);
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
       
        var rollDirection = Mathf.Sign(transform.lossyScale.x);
        _rigidBody.linearVelocity = new Vector2(_rollSpeed * rollDirection, _rigidBody.linearVelocity.y);

        yield return new WaitForSeconds(_rollTime);

        _isRoll = false;
        _reloadScale.Reload();
    }

    IEnumerator DashCrt() 
    {

        _isRoll = true;
        float timePassed = 0;

        var playerLayer = gameObject.layer;
        _layerIgnores.Clear();

        for (var layer = 0; layer < 32; layer++)
        {
            if ((_dashLayers.value & (1 << layer)) == 0) continue;

            var wasIgnored = Physics2D.GetIgnoreLayerCollision(playerLayer, layer);
            _layerIgnores[layer] = wasIgnored;

            Physics2D.IgnoreLayerCollision(playerLayer, layer, true);
        }


        while (timePassed < _beforeDashAnimationTiming) 
        {
            timePassed += Time.deltaTime;
            yield return null;
        }

        var rollDirection = Mathf.Sign(transform.lossyScale.x);
        _rigidBody.linearVelocity = new Vector2(_dashSpeed * rollDirection, _rigidBody.linearVelocity.y);

        yield return new WaitForSeconds(_dashTime);

        foreach (var layer in _layerIgnores)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, layer.Key, layer.Value);
        }
        _layerIgnores.Clear();


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