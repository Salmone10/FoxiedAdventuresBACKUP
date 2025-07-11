using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawnerNoRotation : MonoBehaviour
{  
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
