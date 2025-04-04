using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    [SerializeField] private Transform _objectTransform;
    [SerializeField] private Transform _pointToTeleport;
    [SerializeField] private float _teleportationDelay;

    public void Teleport()
    {
        StartCoroutine(TeleportCrt());
    }

    IEnumerator TeleportCrt()
    {
        float timePassed = 0;

        while (timePassed < _teleportationDelay)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }

        _objectTransform.position = _pointToTeleport.position;
    }
}
