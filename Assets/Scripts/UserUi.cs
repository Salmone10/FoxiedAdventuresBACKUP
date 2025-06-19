using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UserUi : MonoBehaviour
{
    private TextMeshProUGUI _textCountScore;
    private Transform _HPpanelPosition;
    private GridLayoutGroup _HPpanelLayoutGroup;

    [SerializeField] private GameObject _player;

    [SerializeField] private Image[] _cherryImages;
    [SerializeField] private Image[] _fireballImages;
    [SerializeField] private GameObject _HPImagePrfb;

    private PlayerStatistic _playerStatistic;
    private HealthManager _healthManager;

    private void Start()
    {
        _playerStatistic = GetComponent<PlayerStatistic>();
        _healthManager = GetComponent<HealthManager>();
        _textCountScore = FindObjectOfType<ScoreTextNONE>().GetComponent<TextMeshProUGUI>();
        _HPpanelPosition = FindObjectOfType<HPpanelNONE>().GetComponent<Transform>();
        _HPpanelLayoutGroup = FindObjectOfType<HPpanelNONE>().GetComponent<GridLayoutGroup>();
        HideHealthUnit();
    }

    void Update()
    {
        _textCountScore.text = _playerStatistic._score.ToString();
        ShowFireballUnit();
    }

    public void HideHealthUnit()
    {
        if (_healthManager._maxHealth > 5) 
        {
            _HPpanelLayoutGroup.constraintCount = 2;
        }

        if (_HPpanelPosition.childCount > 0) 
        {
            foreach (Transform child in _HPpanelPosition)
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < _healthManager._currentHealth; i++)
        {
            Instantiate(_HPImagePrfb, _HPpanelPosition);
        }       
    }

    public void ShowFireballUnit()
    {
        if (gameObject.CompareTag("DragonPlayer")) 
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
}
