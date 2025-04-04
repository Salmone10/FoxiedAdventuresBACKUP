using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Vector2 _direction;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;

    private Animator _animator;

    private void Start()
    {
       
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool("is_moving", true);
        transform.position += (Vector3)_direction * _speed * Time.deltaTime;
    }


    IEnumerator LifeTimeCrt() 
    {
        yield return new WaitForSeconds(_lifeTime);
        _animator.SetTrigger("is_time_to_explode");
        _speed = 0;
    }

    public void LifeTime() 
    {
        _animator.SetTrigger("is_time_to_explode");
        _speed = 0;
    }
}
