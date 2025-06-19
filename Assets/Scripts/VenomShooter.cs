using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomShooter : MonoBehaviour
{
    [SerializeField] private GameObject _venomSpit;
    [SerializeField] private float _shootForce;

    public void Shoot(Vector2 direction) 
    {
        var venomSpit = Instantiate(_venomSpit, transform.position, Quaternion.identity);
        var rigidBody = venomSpit.GetComponent<Rigidbody2D>();
        var shootDirection = new Vector2(direction.x, 1).normalized;
        //venomSpit.transform.localScale = transform.localScale;
        //rigidBody.AddForce(transform.localScale * _shootForce, ForceMode2D.Impulse);
        rigidBody.velocity = shootDirection * _shootForce;
    }


}
