using System.Collections.Generic;
using Agava.WebUtility;
using UI;
using UnityEngine;

namespace Controller
{
    public class StartMenuSoundMuter : MonoBehaviour
    {
        protected const float VolumeOff = 0f;
        protected const float VolumeOn = 1f;

        [SerializeField] private SoundButton _soundButton;
        [SerializeField] private SoundButton _musicButton;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private List<AudioSource> _soundSources;

        protected virtual void OnEnable()
        {
            _soundButton.SoundButtonClicked += OnSoundButtonClick;
            _musicButton.SoundButtonClicked += OnMusicButtonClick;
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
        }

        protected virtual void OnDisable()
        {
            _soundButton.SoundButtonClicked -= OnSoundButtonClick;
            _musicButton.SoundButtonClicked -= OnMusicButtonClick;
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
        }

        protected virtual void OnInBackgroundChange(bool inBackground)
        {
            AudioListener.volume = inBackground ? VolumeOff : VolumeOn;
            AudioListener.pause = inBackground;
        }

        private void OnMusicButtonClick(bool isMuted)
        {
            _musicSource.mute = isMuted;
        }

        private void OnSoundButtonClick(bool isMuted)
        {
            foreach (var source in _soundSources)
            {
                source.mute = isMuted;
            }
        }
    }
}