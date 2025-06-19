using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReloadScale : MonoBehaviour
{
    [SerializeField] private float _reloadTime;
    [SerializeField] private PlayerController _playerController;
    private Image _scaleImage;

    private void Start() 
    {
        _scaleImage = FindObjectOfType<ScaleNONE>().GetComponent<Image>();
    }

    IEnumerator ReloadCrt()
    {
        _playerController._isRollLocked = true;
        _playerController._isRoll = false;

        float timePassed = 0;
        while (timePassed < _reloadTime) 
        {
            timePassed += Time.deltaTime;
            float progress = Mathf.Clamp01(timePassed / _reloadTime);
            _scaleImage.fillAmount = progress;
            yield return null;
        }
        _playerController._isRollLocked = false;
    }

    public void Reload() 
    {
        StartCoroutine(ReloadCrt());
    }
}
