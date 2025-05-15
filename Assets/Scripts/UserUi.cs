using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textCountScore;
    [SerializeField] private GameObject _player;

    [SerializeField] private Image[] _cherryImages;
    [SerializeField] private Image[] _fireballImages;

    private PlayerStatistic _playerStatistic;
    private HealthManager _healthManager;

    private void Start()
    {
        _playerStatistic = GetComponent<PlayerStatistic>();
        _healthManager = GetComponent<HealthManager>();
    }

    void Update()
    {
        _textCountScore.text = _playerStatistic._score.ToString();
        HideHealthUnit();
        ShowFireballUnit();
    }

    public void HideHealthUnit()
    {
        for (int i = 0; i < _cherryImages.Length; i++)
        {
            _cherryImages[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _healthManager._currentHealth; i++)
        {
            _cherryImages[i].gameObject.SetActive(true);
        }
    }

    public void ShowFireballUnit()
    {
        for (int i = 0; i < _fireballImages.Length; i++)
        {
            var color = _fireballImages[i].color;
            color.a = 0.5f;
            _fireballImages[i].color = color;
        }

        for (int i = 0; i < _playerStatistic._fireballAmmo; i++) 
        {
            var color = _fireballImages[i].color;
            color.a = 1f;
            _fireballImages[i].color = color;
        }
    }
}
