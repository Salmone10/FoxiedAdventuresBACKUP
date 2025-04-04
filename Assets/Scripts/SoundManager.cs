using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.Mathematics;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _slider;
    [SerializeField] private UnityEvent<bool> _onMute;
    private float _volume;

    private void Start()
    {
        /*var settedVolume = PlayerPrefs.;*/
        var isMuted = PlayerPrefs.GetInt("Muted", 1) == 1;
        _onMute?.Invoke(isMuted);
    }

    public void SetVolume() 
    {
        
    }

    public void SetMute(bool isMuted) 
    {
        _mixer.SetFloat("Master", isMuted ? -80f : Mathf.Log10(_volume) * 20);
        PlayerPrefs.SetInt("Muted", isMuted ?  1 : 0);
        PlayerPrefs.Save();
    }

    
}
