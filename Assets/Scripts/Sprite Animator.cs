using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _frameRate; //сколько кадров в секунду мы хотим

    [SerializeField] private int _currentFrame; //текущий кадр из массива
    [SerializeField] private float _timeUntilTheNextFrameChange; //таймер для смены кадра 

    private float _timeSpentPerFrame;

    [SerializeField] private bool _loop;
    private bool _isPlaying = true;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _timeSpentPerFrame = 1f / _frameRate;
    }
    private void Update()
    {
        if (_sprites.Length == 0 || !_isPlaying) { return; }

        _timeUntilTheNextFrameChange += Time.deltaTime;

        if (_timeUntilTheNextFrameChange >= _timeSpentPerFrame)
        {
            _spriteRenderer.sprite = _sprites[_currentFrame];
            _currentFrame++;
            if (_sprites.Length == _currentFrame)
            {
                if (_loop)
                {
                    _currentFrame = 0;
                }
                else
                {
                    _isPlaying = false;
                }
            }
            _timeUntilTheNextFrameChange = 0f;
        }
    }
}
