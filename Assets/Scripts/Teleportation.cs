using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    private Transform _objectTransform;
    [SerializeField] private Transform _pointToTeleport;
    [SerializeField] private float _teleportationDelay;

    private void Start()
    {
        _objectTransform = FindObjectOfType<PlayerController>().GetComponent<Transform>();
    }

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
