using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    [SerializeField] public UnityEvent _onDie;
    [SerializeField] public UnityEvent _onHeal;
    [SerializeField] public UnityEvent _onDamage;

    [SerializeField] public int _currentHealth;
    [SerializeField] public int _maxHealth;
    [SerializeField] public float _invincibleTime;

    [SerializeField] public bool _invincible;

    private int _minHealth = 0;

    public void Apply(int value)
    {
        if (!_invincible)
            if (value > 0 && _currentHealth < _maxHealth)
            {
                if (_currentHealth + value > _maxHealth)
                {
                    _currentHealth = _maxHealth;
                }
                _currentHealth += value;
                _onHeal?.Invoke();

            }
            else if (value < 0)
            {
                if (_currentHealth + value < _minHealth)
                {
                    _currentHealth = _minHealth;
                }
                _currentHealth += value;
                _onDamage?.Invoke();
            }

        if (_currentHealth <= _minHealth)
        {
            _onDie?.Invoke();
        }
    }

    public void GetInvincible()
    {
        StartCoroutine(GetInvincibleCrt());
    }

    IEnumerator GetInvincibleCrt()
    {
        float timePassed = 0;

        while (timePassed < _invincibleTime)
        {
            _invincible = true;
            timePassed += Time.deltaTime;

            yield return null;
        }
        _invincible = false;
    }
}

