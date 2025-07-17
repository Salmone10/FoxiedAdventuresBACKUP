using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawnerNoRotation : MonoBehaviour
{

    public int _collisionCounter;

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    private void OnParticleCollision(GameObject other)
    {
        _collisionCounter++;

        print("{debug} " + _collisionCounter);
        if (_collisionCounter == 10) 
        {
            Destroy(gameObject);
        }


    }
}
