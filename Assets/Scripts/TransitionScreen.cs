using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScreen : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private float targetAlpha = 1f;
    [SerializeField] private float _colorFadeTime;
    [SerializeField] private float _waitingTime;

    private bool _canTransit = true;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartTransition()
    {
        if (_canTransit)  {StartCoroutine(TransitionCrt()); }
    }

    IEnumerator TransitionCrt()
    {
        _canTransit = false;

        float timePassed = 0;
        Color startColor = _spriteRenderer.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (timePassed < _colorFadeTime)
        {
            _spriteRenderer.color = Color.Lerp(startColor, targetColor, timePassed / _colorFadeTime);
            timePassed += Time.deltaTime;
            yield return null;
        }
        _spriteRenderer.color = targetColor;

        yield return new WaitForSeconds(_waitingTime); 

        timePassed = 0;
        Color currentColor = _spriteRenderer.color;
        Color originalColor = new Color(currentColor.r, currentColor.g, currentColor.b, startColor.a);

        while (timePassed < _colorFadeTime)
        {
            _spriteRenderer.color = Color.Lerp(currentColor, originalColor, timePassed / _colorFadeTime);
            timePassed += Time.deltaTime;
            yield return null;
        }
        _spriteRenderer.color = originalColor;

        _canTransit = true;
    }
}