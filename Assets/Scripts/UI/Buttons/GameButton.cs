using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(AudioSource))]
    public class GameButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private AudioSource _audioSource;

        public event Action Clicked;

        protected Button Button => _button;

        protected virtual void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        protected virtual void OnButtonClick()
        {
            _audioSource.Play();
            Clicked?.Invoke();
        }
    }
}