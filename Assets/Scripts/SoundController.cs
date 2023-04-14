using System.Collections.Generic;
using UnityEngine;
using Agava.WebUtility;

public class SoundController : MonoBehaviour
{
    [SerializeField] private InterstitialAdShower _interstitialAdShower;
    [SerializeField] private MusicButton _soundButton;
    [SerializeField] private MusicButton _musicButton;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private List<AudioSource> _soundSources;

    private const float _volumeOff = 0f;
    private const float _volumeOn = 1f;
    private bool _adShowing = false;

    private void OnEnable()
    {
        _soundButton.Clicked += OnSoundButtonClick;
        _musicButton.Clicked += OnMusicButtonClick;
        _interstitialAdShower.AdShowing += OnAdShowed;
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
    }

    private void OnDisable()
    {
        _soundButton.Clicked -= OnSoundButtonClick;
        _musicButton.Clicked -= OnMusicButtonClick;
        _interstitialAdShower.AdShowing -= OnAdShowed;
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
    }

    private void OnMusicButtonClick(bool isMuted)
    {
        if (isMuted)
            _musicSource.mute = true;
        else
            _musicSource.mute = false;
    }

    private void OnSoundButtonClick(bool isMuted)
    {
        if (isMuted)
            foreach (var sound in _soundSources)
                sound.mute = true;
        else
            foreach (var sound in _soundSources)
                sound.mute = false;
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        if (inBackground)
        {
            AudioListener.volume = _volumeOff;
            AudioListener.pause = inBackground;
        }
        else if (_adShowing == false)
        {
            AudioListener.volume = _volumeOn;
            AudioListener.pause = inBackground;
        }
    }

    private void OnAdShowed(bool showing)
    {
        _adShowing = showing;
        AudioListener.pause = showing;
        AudioListener.volume = showing ? _volumeOff : _volumeOn;
    }
}
