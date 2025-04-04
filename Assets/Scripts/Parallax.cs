using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _parallaxFactorX;
    [SerializeField] private float _parallaxFactorY;

    private Transform _cameraTransform;
    private Vector3 _previousCameraPosition;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _previousCameraPosition = _cameraTransform.position;
    }

    private void FixedUpdate()
    {
        Vector3 delta = _cameraTransform.position - _previousCameraPosition;
        transform.position += new Vector3(delta.x * _parallaxFactorX, delta.y * _parallaxFactorY, 0);
        _previousCameraPosition = _cameraTransform.position;
    }
}
