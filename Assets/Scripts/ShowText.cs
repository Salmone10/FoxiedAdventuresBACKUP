
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private float _colorFadeTime;
    private float _targetAlpha = 1f;
    private float _currentAlpha = 0f;
    [SerializeField] private bool _isSmoothBlinking = true;

    public void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void SetTextAlpha(float alpha)
    {
        Color color = _text.color;
        color.a = alpha;
        _text.color = color;
    }

    private void OnEnable()
    {
        StartCoroutine(BlinkTextCrt());
    }

    IEnumerator BlinkTextCrt()
    {
        while (true)
        {
            if (_isSmoothBlinking)
            {
                yield return StartCoroutine(FadeAlpha(0f));
                yield return new WaitForSeconds(_colorFadeTime);

                yield return StartCoroutine(FadeAlpha(1f));
                yield return new WaitForSeconds(_colorFadeTime);
            }

            else
            {
                SetTextAlpha(0f);
                yield return new WaitForSeconds(_colorFadeTime);

                SetTextAlpha(1f);
                yield return new WaitForSeconds(_colorFadeTime);
            }
        }
    }

    IEnumerator FadeAlpha(float targetAlpha)
    {
        while (!Mathf.Approximately(_currentAlpha, targetAlpha))
        {
            _currentAlpha = Mathf.MoveTowards(_currentAlpha, targetAlpha, Time.deltaTime / _colorFadeTime);
            SetTextAlpha(_currentAlpha);
            yield return null;
        }
    }

    public void ToggleBlinkMode()
    {
        _isSmoothBlinking = !_isSmoothBlinking;
    }
}