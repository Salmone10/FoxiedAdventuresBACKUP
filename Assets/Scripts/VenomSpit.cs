using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class VenomSpit : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider;
    private ParticleSystem.Particle[] _particlesQuantity;
    private SpriteRenderer _spriteRenderer;
    private bool _checkParticles = false;

    [SerializeField] private ParticleSystem _particleSpawner;
    

    private ParticleSystemRenderer _particleSystemRenderer;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _particlesQuantity = new ParticleSystem.Particle[_particleSpawner.main.maxParticles];
        _particleSystemRenderer = _particleSpawner.GetComponent<ParticleSystemRenderer>();
    }
    private void FixedUpdate()
    {
        if (_rigidBody.linearVelocity.sqrMagnitude > 0.01) 
        {
            var angle = Mathf.Atan2(_rigidBody.linearVelocity.y, Mathf.Abs(_rigidBody.linearVelocity.x)) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? angle : -angle);
            transform.localScale = new Vector3(Mathf.Sign(_rigidBody.linearVelocity.x), 1, 1);
            _particleSystemRenderer.flip = new Vector3(-transform.localScale.x, 0, 0);
        }
    }

    private void Update()
    {
        if (_checkParticles)
        {
            var count = _particleSpawner.GetParticles(_particlesQuantity);
            print(count);

            if (!_particleSpawner.IsAlive(true)) { Destroy(gameObject); }
        }
        
    }
    
    public void SpawnParticles() 
    {
        _rigidBody.bodyType = RigidbodyType2D.Static;
        _collider.enabled = false;
        _spriteRenderer.enabled = false;
        _particleSpawner.gameObject.SetActive(true);
        _particleSpawner.Play();
        _checkParticles = true;
    }

}
