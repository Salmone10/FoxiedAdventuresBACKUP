using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadScale : MonoBehaviour
{
    [SerializeField] private float _reloadTime;
    [SerializeField] private PlayerController _playerController;

    IEnumerator ReloadCrt()
    {
        yield return new WaitForSeconds(_reloadTime);
        _playerController._isRoll = false;
    }

    public void Reload() 
    {
        StartCoroutine(ReloadCrt());
    }

}
