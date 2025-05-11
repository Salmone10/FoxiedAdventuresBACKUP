using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadScale : MonoBehaviour
{
    [SerializeField] private float _reloadTime;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Image _sliderImage;

    IEnumerator ReloadCrt()
    {
        //yield return new WaitForSeconds(_reloadTime);
        _playerController._isRollLocked = true;
        _playerController._isRoll = false;

        float timePassed = 0;
        while (timePassed < _reloadTime) 
        {
            timePassed += Time.deltaTime;
            float progress = Mathf.Clamp01(timePassed / _reloadTime);
            _sliderImage.fillAmount = progress;
            yield return null;
        }
        _playerController._isRollLocked = false;
    }

    public void Reload() 
    {
        StartCoroutine(ReloadCrt());
    }
}
