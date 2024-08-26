using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundsView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private bool _isSound;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private SoundsView _otherSliderView;

    private bool _isSyncing = false; 

    private void Awake()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        if (_isSyncing) return;

        _isSyncing = true;

        if (_isSound)
        {
            _mixer.SetFloat("SoundsVolume", Mathf.Log10(value) * 20);
        }
        else
        {
            _mixer.SetFloat("MusicsVolume", Mathf.Log10(value) * 20);
        }

        if (_otherSliderView != null && _otherSliderView._slider.value != value)
        {
            _otherSliderView.SyncSliderValue(value);
        }

        _isSyncing = false;
    }

    public void SyncSliderValue(float value)
    {
        _isSyncing = true;
        _slider.value = value;
        _isSyncing = false;
    }
}
