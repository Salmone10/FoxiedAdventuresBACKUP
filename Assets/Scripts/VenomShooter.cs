using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomShooter : MonoBehaviour
{
    [SerializeField] private GameObject _venomSpit;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _shootForce;
    [SerializeField] private float _offsetY;

    public void Shoot() 
    {
        var venomSpit = Instantiate(_venomSpit, _spawnPoint.position, Quaternion.identity);
        var rigidBody = venomSpit.GetComponent<Rigidbody2D>();
        var shootDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        shootDirection += Vector2.up * _offsetY;
        rigidBody.AddForce(shootDirection.normalized * _shootForce, ForceMode2D.Impulse);        
    }



}
