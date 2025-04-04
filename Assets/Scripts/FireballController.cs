using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField] private Transform _fireStartPoint;
    [SerializeField] private Fireball _fireball;
    [SerializeField] private float _ammoRecoveryTime;

    private PlayerStatistic _playerStatistic;
    private Animator _animator;
    private UserUi _userUi;

    public void Start()
    {
        _playerStatistic = GetComponent<PlayerStatistic>();
        _userUi = GetComponent<UserUi>();
        _animator = GetComponent<Animator>();
        StartCoroutine(AmmoRecoveryCrt());
    }

    public void MakeFireball()
    {
        if (_playerStatistic._fireballAmmo > 0) 
        {
            var fireball = Instantiate(_fireball, _fireStartPoint.position, Quaternion.identity );
            _animator.SetTrigger("is_shooting");

            if (transform.localScale.x > 0)
            {
                fireball._direction = Vector2.right;
                fireball.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else 
            {
                fireball._direction = Vector2.left;
                fireball.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            _playerStatistic._fireballAmmo--;
        }
    }

    IEnumerator AmmoRecoveryCrt() 
    {
        
        while (true) 
        {
            if (_playerStatistic._fireballAmmo < _playerStatistic._fireballClipSize)
            {
                _playerStatistic._fireballAmmo++;
                yield return new WaitForSeconds(_ammoRecoveryTime);
            }
            else 
            {
                yield return null;
            }
        }
        
    }
}
