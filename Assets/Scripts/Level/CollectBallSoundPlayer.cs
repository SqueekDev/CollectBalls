using Global;
using UnityEngine;

namespace Level
{
    public class CollectBallSoundPlayer : MonoBehaviour
    {
        private const int NumberToPlaySound = 1;
        private const float MinScale = 0.3f;
        private const float MaxScale = 1f;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;

        private float _volumeScale;
        private int _soundCounter = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ball _))
            {
                PlaySound();
            }
        }

        private void PlaySound()
        {
            if (_soundCounter > NumberToPlaySound)
            {
                _volumeScale = Random.Range(MinScale, MaxScale);
                _audioSource.PlayOneShot(_audioClip, _volumeScale);
                _soundCounter = GlobalValues.Zero;
            }
            else
            {
                _soundCounter++;
            }
        }
    }
}