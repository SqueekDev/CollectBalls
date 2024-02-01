using Controller;
using UnityEngine;

namespace Level
{
    public class FenceSoundPlayer : MonoBehaviour
    {
        [SerializeField] private FenceSelector _fenceSelector;
        [SerializeField] private AudioSource _audioSource;

        private void OnEnable()
        {
            _fenceSelector.Selected += OnFenceSelected;
            _fenceSelector.Unselected += OnFenceUnselected;
        }

        private void OnDisable()
        {
            _fenceSelector.Selected -= OnFenceSelected;
            _fenceSelector.Unselected -= OnFenceUnselected;
        }

        private void OnFenceSelected(IronFenceSwiper fence, Vector3 tapPosition)
        {
            fence.Sounded += OnFenceSounded;
        }

        private void OnFenceUnselected(IronFenceSwiper fence)
        {
            fence.Sounded -= OnFenceSounded;
        }

        private void OnFenceSounded(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
        }
    }
}