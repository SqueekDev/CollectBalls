using System.Collections.Generic;
using UnityEngine;
using Agava.WebUtility;

public class StartMenuSoundMuter : MonoBehaviour
{
    protected const float VolumeOff = 0f;
    protected const float VolumeOn = 1f;

    [SerializeField] private MusicButton _soundButton;
    [SerializeField] private MusicButton _musicButton;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private List<AudioSource> _soundSources;

    protected virtual void OnEnable()
    {
        _soundButton.Clicked += OnSoundButtonClick;
        _musicButton.Clicked += OnMusicButtonClick;
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
    }

    protected virtual void OnDisable()
    {
        _soundButton.Clicked -= OnSoundButtonClick;
        _musicButton.Clicked -= OnMusicButtonClick;
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
    }

    protected virtual void OnInBackgroundChange(bool inBackground)
    {
        if (inBackground)
        {
            AudioListener.volume = VolumeOff;
            AudioListener.pause = inBackground;
        }
        else
        {
            AudioListener.volume = VolumeOn;
            AudioListener.pause = inBackground;
        }
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
}
